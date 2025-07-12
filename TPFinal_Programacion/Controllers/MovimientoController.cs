using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPFinal_Programacion.DTOs;
using TPFinal_Programacion.Models;

namespace TPFinal_Programacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientoController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MovimientoController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovimientoDTO>>> Get()
        {
            var movimientos = await _context.Movimientos.Include(x => x.Cliente).ToListAsync();
            var movimientoDto = movimientos.Select(m => new MovimientoDTO
            {
                Id = m.Id,
                CryptoCode = m.CryptoCode,
                Action = m.Action,
                CryptoAmount = m.CryptoAmount,
                Pesos = m.Pesos,
                DateTime = m.DateTime,
                Cliente = m.Cliente.Nombre
            }).ToList();

            return Ok(movimientoDto);
        }
        [HttpPost]
        public async Task<ActionResult<Movimiento>> Post(MovimientoPostDTO dto)
        {
            using var client = new HttpClient();

            string url = $"https://criptoya.com/api/satoshitango/{dto.CryptoCode.ToLower()}/ars";

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return BadRequest();

            var json = await response.Content.ReadAsStringAsync();

            var askValue = System.Text.Json.JsonDocument.Parse(json).RootElement.GetProperty("ask").GetDecimal();

            decimal totalEnPesos = askValue * dto.CryptoAmount;

            if (dto.Action.ToLower() == "sale")
            {
                var totalComprado = await _context.Movimientos
                    .Where(m => m.ClienteId == dto.ClienteId && m.CryptoCode.ToLower() == dto.CryptoCode.ToLower() && m.Action.ToLower() == "purchase")
                    .SumAsync(m => m.CryptoAmount);
                
                var totalVendido = await _context.Movimientos
                    .Where(m => m.ClienteId == dto.ClienteId && m.CryptoCode.ToLower() == dto.CryptoCode.ToLower() && m.Action.ToLower() == "sale")
                    .SumAsync (m => m.CryptoAmount);

                var saldoDisponible = totalComprado - totalVendido;

                if (dto.CryptoAmount > saldoDisponible)
                {
                    return BadRequest($"No tenes suficiente saldo de {dto.CryptoCode}. Saldo disponible: {saldoDisponible}");
                }
            }

            var movimiento = new Movimiento
            {
                CryptoCode = dto.CryptoCode,
                Action = dto.Action,
                CryptoAmount = dto.CryptoAmount,
                DateTime = dto.DateTime,
                ClienteId = dto.ClienteId,
                Pesos = totalEnPesos
            };

            _context.Movimientos.Add(movimiento);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = movimiento.Id }, movimiento);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<MovimientoDTO>> Get(int id)
        {
            var movimiento = await _context.Movimientos.FirstOrDefaultAsync(m => m.Id == id);
            if (movimiento == null)
                return BadRequest();
            var dto = new MovimientoDTO
            {
                Id = movimiento.Id,
                CryptoCode = movimiento.CryptoCode,
                Action = movimiento.Action,
                CryptoAmount = movimiento.CryptoAmount,
                Pesos = movimiento.Pesos,
                DateTime = movimiento.DateTime
            };

            return Ok(dto);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Movimiento movimiento)
        {
            if (id != movimiento.Id)
            {
                return BadRequest();
            }
            _context.Entry(movimiento).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var movimiento = await _context.Movimientos.FindAsync(id);
            if (movimiento == null)
            {
                return NotFound();
            }
            _context.Movimientos.Remove(movimiento);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
