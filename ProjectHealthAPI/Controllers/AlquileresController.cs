using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHealthAPI.Data;
using ProjectHealthAPI.Models;

namespace ProjectHealthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlquileresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AlquileresController(AppDbContext context)
        {
            _context = context;
        }

        // PUERTA 1: Obtener todas las Alquileres (CON los datos del Cliente)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alquiler>>> GetAlquileres()
        {
            // El .Include() es la magia: trae la Alquiler y "adjunta" al Cliente dueño de ese ID
            return await _context.Alquileres
                                 .Include(c => c.Cliente)
                                 .ToListAsync();
        }

        // PUERTA 2: Buscar una Alquiler por ID (CON los datos del Cliente)
        [HttpGet("{id}")]
        public async Task<ActionResult<Alquiler>> GetAlquiler(int id)
        {
            var Alquiler = await _context.Alquileres
                                     .Include(c => c.Cliente)
                                     .FirstOrDefaultAsync(c => c.Id == id);

            if (Alquiler == null)
            {
                return NotFound("Alquiler no encontrada.");
            }

            return Alquiler;
        }

        // PUERTA 3: Crear una nueva Alquiler
        [HttpPost]
        public async Task<ActionResult<Alquiler>> PostAlquiler(Alquiler Alquiler)
        {
            // Al crear, solo necesitas enviar el ClienteId (ej: "ClienteId": 1) en el JSON
            _context.Alquileres.Add(Alquiler);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAlquiler), new { id = Alquiler.Id }, Alquiler);
        }

        // PUERTA 4: Actualizar Alquiler
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlquiler(int id, Alquiler Alquiler)
        {
            if (id != Alquiler.Id) return BadRequest();

            _context.Entry(Alquiler).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUERTA 5: Eliminar Alquiler
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlquiler(int id)
        {
            var Alquiler = await _context.Alquileres.FindAsync(id);
            if (Alquiler == null) return NotFound();

            _context.Alquileres.Remove(Alquiler);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}