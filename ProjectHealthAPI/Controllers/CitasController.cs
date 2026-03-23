using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHealthAPI.Data;
using ProjectHealthAPI.Models;
using ProjectHealthAPI.DTOs;

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

        // PUERTA 1: Obtener todas las Citas (GET) - BLINDADA Y APLANADA
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CitaResponseDTO>>> GetCitas()
        {
            // 1. Buscamos las citas e INCLUIMOS al papá (Paciente)
            var citas = await _context.Citas
                                      .Include(c => c.Paciente) 
                                      .ToListAsync();

            // 2. TRADUCCIÓN MASIVA Y APLANAMIENTO
            var respuesta = citas.Select(c => new CitaResponseDTO
            {
                Id = c.Id,
                Fecha = c.Fecha,
                HoraEntrada = c.HoraEntrada,
                HoraSalida = c.HoraSalida,
                EstadoServicio = (int)c.EstadoServicio,
                
                // 🪄 Sacamos los datos del padre para que Flutter no tenga que hacer otra consulta
                PacienteId = c.PacienteId,
                PacienteNombre = c.Paciente != null ? $"{c.Paciente.Nombre} {c.Paciente.Apellido}" : "Desconocido",
                PacienteCedula = c.Paciente != null ? c.Paciente.Cedula : "N/A"
            }).ToList();

            return Ok(respuesta);
        }
        // PUERTA 2: Crear una nueva Cita (POST) - BLINDADA
        [HttpPost]
        public async Task<ActionResult<CitaResponseDTO>> PostCita(CitaCreateDTO citaDTO)
        {
            // 🛡️ VALIDACIÓN EXTRA: Verificamos que el paciente exista antes de crearle la cita
            var paciente = await _context.Pacientes.FindAsync(citaDTO.PacienteId);
            if (paciente == null)
            {
                return BadRequest("El paciente especificado no existe.");
            }

            // 1. TRADUCCIÓN (Mapeo)
            var nuevaCita = new Cita
            {
                PacienteId = citaDTO.PacienteId,
                Fecha = citaDTO.Fecha,
                HoraEntrada = citaDTO.HoraEntrada,
                HoraSalida = citaDTO.HoraSalida,
                
                // Forzamos el estado a 0 (Agendada) por seguridad
                EstadoServicio = (EstadoServicio)0 
            };

            // 2. GUARDAR
            _context.Citas.Add(nuevaCita);
            await _context.SaveChangesAsync();

            // 3. ARMAR LA RESPUESTA APLANADA
            // Como ya buscamos al paciente arriba para validarlo, usamos sus datos aquí directamente
            var respuesta = new CitaResponseDTO
            {
                Id = nuevaCita.Id,
                Fecha = nuevaCita.Fecha,
                HoraEntrada = nuevaCita.HoraEntrada,
                HoraSalida = nuevaCita.HoraSalida,
                EstadoServicio = (int)nuevaCita.EstadoServicio,
                
                // 🪄 APLANAMIENTO LISTO PARA FLUTTER
                PacienteId = paciente.Id,
                PacienteNombre = $"{paciente.Nombre} {paciente.Apellido}",
                PacienteCedula = paciente.Cedula
            };

            return CreatedAtAction(nameof(GetCita), new { id = nuevaCita.Id }, respuesta);
        }

        // PUERTA 3: Buscar una Cita por ID (GET) - BLINDADA Y APLANADA
        [HttpGet("{id}")]
        public async Task<ActionResult<CitaResponseDTO>> GetCita(int id)
        {
            // Buscamos la cita e incluimos los datos del padre
            var cita = await _context.Citas
                                     .Include(c => c.Paciente)
                                     .FirstOrDefaultAsync(c => c.Id == id);

            if (cita == null) return NotFound("Cita no encontrada.");

            // TRADUCCIÓN INDIVIDUAL
            var respuesta = new CitaResponseDTO
            {
                Id = cita.Id,
                Fecha = cita.Fecha,
                HoraEntrada = cita.HoraEntrada,
                HoraSalida = cita.HoraSalida,
                EstadoServicio = (int)cita.EstadoServicio,
                
                PacienteId = cita.PacienteId,
                PacienteNombre = cita.Paciente != null ? $"{cita.Paciente.Nombre} {cita.Paciente.Apellido}" : "Desconocido",
                PacienteCedula = cita.Paciente != null ? cita.Paciente.Cedula : "N/A"
            };

            return Ok(respuesta);
        }

        // PUERTA 4: Actualizar Cita (PUT) - BLINDADA
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCita(int id, CitaCreateDTO citaDTO)
        {
            // 1. Buscamos la cita original
            var citaBd = await _context.Citas.FindAsync(id);
            if (citaBd == null) return NotFound("Cita no encontrada.");

            // 🛡️ VALIDACIÓN EXTRA: Si el usuario intenta cambiar la cita a otro paciente, verificamos que el nuevo exista
            if (citaBd.PacienteId != citaDTO.PacienteId)
            {
                var pacienteExiste = await _context.Pacientes.AnyAsync(p => p.Id == citaDTO.PacienteId);
                if (!pacienteExiste) return BadRequest("El nuevo paciente especificado no existe.");
            }

            // 2. TRADUCCIÓN (Sobreescritura segura)
            citaBd.PacienteId = citaDTO.PacienteId;
            citaBd.Fecha = citaDTO.Fecha;
            citaBd.HoraEntrada = citaDTO.HoraEntrada;
            citaBd.HoraSalida = citaDTO.HoraSalida;
            
            // Fíjate que NO tocamos citaBd.EstadoServicio. Eso requeriría otro endpoint o permisos especiales.

            // 3. Guardar cambios
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