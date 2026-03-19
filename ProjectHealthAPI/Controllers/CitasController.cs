using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHealthAPI.Data;
using ProjectHealthAPI.Models;

namespace ProjectHealthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CitasController(AppDbContext context)
        {
            _context = context;
        }

        // PUERTA 1: Obtener todas las citas (CON los datos del paciente)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cita>>> GetCitas()
        {
            // El .Include() es la magia: trae la cita y "adjunta" al paciente dueño de ese ID
            return await _context.Citas
                                 .Include(c => c.Paciente)
                                 .ToListAsync();
        }

        // PUERTA 2: Buscar una cita por ID (CON los datos del paciente)
        [HttpGet("{id}")]
        public async Task<ActionResult<Cita>> GetCita(int id)
        {
            var cita = await _context.Citas
                                     .Include(c => c.Paciente)
                                     .FirstOrDefaultAsync(c => c.Id == id);

            if (cita == null)
            {
                return NotFound("Cita no encontrada.");
            }

            return cita;
        }

        // PUERTA 3: Crear una nueva cita
        [HttpPost]
        public async Task<ActionResult<Cita>> PostCita(Cita cita)
        {
            // Al crear, solo necesitas enviar el PacienteId (ej: "PacienteId": 1) en el JSON
            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCita), new { id = cita.Id }, cita);
        }

        // PUERTA 4: Actualizar Cita
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCita(int id, Cita cita)
        {
            if (id != cita.Id) return BadRequest();

            _context.Entry(cita).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUERTA 5: Eliminar Cita
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCita(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null) return NotFound();

            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}