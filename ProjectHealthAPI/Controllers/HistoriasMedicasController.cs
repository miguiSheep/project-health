using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHealthAPI.Data;
using ProjectHealthAPI.Models;

namespace ProjectHealthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoriasMedicasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HistoriasMedicasController(AppDbContext context)
        {
            _context = context;
        }

        // PUERTA 1: Obtener todas las HistoriasMedicas (CON los datos del paciente)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistoriaMedica>>> GetHistoriasMedicas()
        {
            // El .Include() es la magia: trae la HistoriaMedica y "adjunta" al paciente dueño de ese ID
            return await _context.HistoriasMedicas
                                 .Include(c => c.Paciente)
                                 .ToListAsync();
        }

        // PUERTA 2: Buscar una HistoriaMedica por ID (CON los datos del paciente)
        [HttpGet("{id}")]
        public async Task<ActionResult<HistoriaMedica>> GetHistoriaMedica(int id)
        {
            var HistoriaMedica = await _context.HistoriasMedicas
                                     .Include(c => c.Paciente)
                                     .FirstOrDefaultAsync(c => c.Id == id);

            if (HistoriaMedica == null)
            {
                return NotFound("HistoriaMedica no encontrada.");
            }

            return HistoriaMedica;
        }

        // PUERTA 3: Crear una nueva HistoriaMedica
        [HttpPost]
        public async Task<ActionResult<HistoriaMedica>> PostHistoriaMedica(HistoriaMedica HistoriaMedica)
        {
            // Al crear, solo necesitas enviar el PacienteId (ej: "PacienteId": 1) en el JSON
            _context.HistoriasMedicas.Add(HistoriaMedica);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHistoriaMedica), new { id = HistoriaMedica.Id }, HistoriaMedica);
        }

        // PUERTA 4: Actualizar HistoriaMedica
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHistoriaMedica(int id, HistoriaMedica HistoriaMedica)
        {
            if (id != HistoriaMedica.Id) return BadRequest();

            _context.Entry(HistoriaMedica).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUERTA 5: Eliminar HistoriaMedica
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistoriaMedica(int id)
        {
            var HistoriaMedica = await _context.HistoriasMedicas.FindAsync(id);
            if (HistoriaMedica == null) return NotFound();

            _context.HistoriasMedicas.Remove(HistoriaMedica);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}