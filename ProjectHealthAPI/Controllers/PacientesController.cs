using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHealthAPI.Data;
using ProjectHealthAPI.Models;

namespace ProjectHealthAPI.Controllers
{
    // Esta línea le dice a internet cómo llegar aquí: "tusitio.com/api/Pacientes"
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        // "Inyección de dependencias": Le pasamos las llaves de la base de datos al controlador
        public PacientesController(AppDbContext context)
        {
            _context = context;
        }

        // PUERTA 1: Obtener todos los pacientes (GET)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paciente>>> GetPacientes()
        {
            // Va a la base de datos, busca la tabla Pacientes y la devuelve como una lista
            return await _context.Pacientes.ToListAsync();
        }

        // PUERTA 2: Crear un nuevo paciente (POST)
        [HttpPost]
        public async Task<ActionResult<Paciente>> PostPaciente(Paciente paciente)
        {
            // Agrega el paciente que mandaste a la tabla
            _context.Pacientes.Add(paciente);
            // Guarda los cambios en PostgreSQL
            await _context.SaveChangesAsync();

            // Te devuelve un mensaje de éxito ("Ok") con los datos guardados
            return Ok(paciente);
        }
        // PUERTA 3: Buscar un paciente específico por su ID (GET por ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<Paciente>> GetPaciente(int id)
        {
            // Busca en la tabla el paciente que tenga ese ID exacto
            var paciente = await _context.Pacientes.FindAsync(id);

            // Si no lo encuentra, devuelve un error 404
            if (paciente == null) 
            {
                return NotFound("Paciente no encontrado en la base de datos.");
            }

            return paciente;
        }

        // PUERTA 4: Actualizar o Editar un paciente (PUT)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaciente(int id, Paciente paciente)
        {
            // Medida de seguridad: verificar que el ID de la URL sea el mismo del formulario
            if (id != paciente.Id) 
            {
                return BadRequest("El ID de la URL no coincide con el del paciente.");
            }

            // Le avisa a Entity Framework que este registro fue modificado
            _context.Entry(paciente).State = EntityState.Modified;
            
            // Guarda los cambios en PostgreSQL
            await _context.SaveChangesAsync();

            return NoContent(); // Responde con éxito pero sin devolver texto extra
        }

        // PUERTA 5: Eliminar un paciente (DELETE)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaciente(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null) 
            {
                return NotFound();
            }

            // Lo borra de la tabla
            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}