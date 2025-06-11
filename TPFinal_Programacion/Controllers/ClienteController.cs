using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPFinal_Programacion.DTOs;
using TPFinal_Programacion.Models;

namespace TPFinal_Programacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClienteController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> Get()
        {
            var cliente = await _context.Clientes.Include(m => m.Movimientos).ToListAsync();

            var clienteDto = cliente.Select(c => new ClienteDTO
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Email = c.Email,
                Movimientos = c.Movimientos
                .Where(x => x.ClienteId == c.Id)
                .Select(m => new MovimientoDTO
                {
                    Id = m.Id,
                    CryptoCode = m.CryptoCode,
                    Action = m.Action,
                    CryptoAmount = m.CryptoAmount,
                    Pesos = m.Pesos,
                    DateTime = m.DateTime
                }).ToList()
                
            }).ToList();

            return Ok(clienteDto);
        }
        [HttpPost]
        public async Task<ActionResult<Cliente>> Post(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id  = cliente.Id }, cliente);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDTO>> Get(int id)
        {
            var cliente = await _context.Clientes.Include(m => m.Movimientos).FirstOrDefaultAsync(m => m.Id == id);  
            if (cliente == null)
                return BadRequest();
            var dto = new ClienteDTO
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Email = cliente.Email,
                Movimientos = cliente.Movimientos
                .Where(x =>  x.ClienteId == id)
                .Select(m => new MovimientoDTO
                {
                    Id = m.Id,
                    CryptoCode = m.CryptoCode,
                    Action = m.Action,
                    CryptoAmount = m.CryptoAmount,
                    Pesos = m.Pesos,
                    DateTime = m.DateTime
                }).ToList()
            };

            return Ok(dto);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if( cliente == null)
            {
                return NotFound();
            }
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest();
            }
            _context.Entry(cliente).State= EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
    }
}
