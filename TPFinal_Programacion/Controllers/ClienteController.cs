using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<IEnumerable<Cliente>>> Get();
    }
}
