using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHealthAPI.Data;
using ProjectHealthAPI.Models;
using ProjectHealthAPI.DTOs;

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

        // PUERTA 1: Obtener todas las Historias Médicas (GET) - BLINDADA Y APLANADA
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistoriaMedicaResponseDTO>>> GetHistoriasMedicas()
        {
            var historias = await _context.HistoriasMedicas
                                          .Include(h => h.Paciente) // Traemos los datos del paciente
                                          .ToListAsync();

            var respuesta = historias.Select(h => new HistoriaMedicaResponseDTO
            {
                Id = h.Id,
                
                // 🪄 Casteo Inverso: De Enum (BD) a int (Internet)
                AntecedentesPersonales = (int)h.antecedentesPersonales,
                Farmaco = h.Farmaco,
                Dosificacion = h.Dosificacion,
                Antidepresivos = h.Antidepresivos,
                Ansioliticos = h.Ansioliticos,
                Fumador = (int)h.Fumador,
                EdadInicio = h.EdadInicio,
                ConsumoAlcohol = (int)h.ConsumoAlcohol,
                Ejercicio = (int)h.Ejercicio,
                Traumatismos = h.Traumatismos,
                HobbiesDeportes = h.HobbiesDeportes,
                Diagnostico = h.Diagnostico,
                EscalaDolor = h.EscalaDolor,
                EstadoCliente = (int)h.EstadoCliente,

                // 🪄 APLANAMIENTO: Sacamos la info del Paciente
                PacienteId = h.PacienteId,
                PacienteNombre = h.Paciente != null ? $"{h.Paciente.Nombre} {h.Paciente.Apellido}" : "Desconocido",
                PacienteCedula = h.Paciente != null ? h.Paciente.Cedula : "N/A"
            }).ToList();

            return Ok(respuesta);
        }

        // PUERTA 2: Buscar una Historia por ID (GET) - BLINDADA Y APLANADA
        [HttpGet("{id}")]
        public async Task<ActionResult<HistoriaMedicaResponseDTO>> GetHistoriaMedica(int id)
        {
            var h = await _context.HistoriasMedicas
                                  .Include(p => p.Paciente)
                                  .FirstOrDefaultAsync(p => p.Id == id);

            if (h == null) return NotFound("Historia Médica no encontrada.");

            var respuesta = new HistoriaMedicaResponseDTO
            {
                Id = h.Id,
                AntecedentesPersonales = (int)h.antecedentesPersonales,
                Farmaco = h.Farmaco,
                Dosificacion = h.Dosificacion,
                Antidepresivos = h.Antidepresivos,
                Ansioliticos = h.Ansioliticos,
                Fumador = (int)h.Fumador,
                EdadInicio = h.EdadInicio,
                ConsumoAlcohol = (int)h.ConsumoAlcohol,
                Ejercicio = (int)h.Ejercicio,
                Traumatismos = h.Traumatismos,
                HobbiesDeportes = h.HobbiesDeportes,
                Diagnostico = h.Diagnostico,
                EscalaDolor = h.EscalaDolor,
                EstadoCliente = (int)h.EstadoCliente,

                PacienteId = h.PacienteId,
                PacienteNombre = h.Paciente != null ? $"{h.Paciente.Nombre} {h.Paciente.Apellido}" : "Desconocido",
                PacienteCedula = h.Paciente != null ? h.Paciente.Cedula : "N/A"
            };

            return Ok(respuesta);
        }

        // PUERTA 3: Crear una nueva Historia Médica (POST) - ¡ESCUDO ACTIVADO!
        [HttpPost]
        public async Task<ActionResult<HistoriaMedicaResponseDTO>> PostHistoriaMedica(HistoriaMedicaCreateDTO historiaDTO)
        {
            // 🛡️ VALIDACIÓN EXTRA: ¿El paciente existe en la clínica?
            var paciente = await _context.Pacientes.FindAsync(historiaDTO.PacienteId);
            if (paciente == null) return BadRequest("El paciente especificado no existe.");

            // 1. TRADUCCIÓN (Mapeo): Pasamos los datos del DTO al Modelo
            var nuevaHistoria = new HistoriaMedica
            {
                PacienteId = historiaDTO.PacienteId,
                
                // 🪄 Casteo Directo: De int (Internet) a Enum (BD)
                antecedentesPersonales = (AntecedentesPersonales)historiaDTO.AntecedentesPersonales,
                Farmaco = historiaDTO.Farmaco,
                Dosificacion = historiaDTO.Dosificacion,
                Antidepresivos = historiaDTO.Antidepresivos,
                Ansioliticos = historiaDTO.Ansioliticos,
                Fumador = (Fumador)historiaDTO.Fumador,
                EdadInicio = historiaDTO.EdadInicio,
                ConsumoAlcohol = (ConsumoAlcohol)historiaDTO.ConsumoAlcohol,
                Ejercicio = (Ejercicio)historiaDTO.Ejercicio,
                Traumatismos = historiaDTO.Traumatismos,
                HobbiesDeportes = historiaDTO.HobbiesDeportes,
                Diagnostico = historiaDTO.Diagnostico,
                EscalaDolor = historiaDTO.EscalaDolor,
                
                // 🛡️ REGLA DE NEGOCIO: Nace con estado 0
                EstadoCliente = (EstadoCliente)0 
            };

            // 2. GUARDAR
            _context.HistoriasMedicas.Add(nuevaHistoria);
            await _context.SaveChangesAsync();

            // 3. ARMAR RESPUESTA APLANADA (Usamos el objeto "paciente" que ya buscamos arriba)
            var respuesta = new HistoriaMedicaResponseDTO
            {
                Id = nuevaHistoria.Id,
                AntecedentesPersonales = (int)nuevaHistoria.antecedentesPersonales,
                Farmaco = nuevaHistoria.Farmaco,
                Dosificacion = nuevaHistoria.Dosificacion,
                Antidepresivos = nuevaHistoria.Antidepresivos,
                Ansioliticos = nuevaHistoria.Ansioliticos,
                Fumador = (int)nuevaHistoria.Fumador,
                EdadInicio = nuevaHistoria.EdadInicio,
                ConsumoAlcohol = (int)nuevaHistoria.ConsumoAlcohol,
                Ejercicio = (int)nuevaHistoria.Ejercicio,
                Traumatismos = nuevaHistoria.Traumatismos,
                HobbiesDeportes = nuevaHistoria.HobbiesDeportes,
                Diagnostico = nuevaHistoria.Diagnostico,
                EscalaDolor = nuevaHistoria.EscalaDolor,
                EstadoCliente = (int)nuevaHistoria.EstadoCliente,

                PacienteId = paciente.Id,
                PacienteNombre = $"{paciente.Nombre} {paciente.Apellido}",
                PacienteCedula = paciente.Cedula
            };

            return CreatedAtAction(nameof(GetHistoriaMedica), new { id = nuevaHistoria.Id }, respuesta);
        }

        // PUERTA 4: Actualizar Historia Médica (PUT) - BLINDADA
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHistoriaMedica(int id, HistoriaMedicaCreateDTO historiaDTO)
        {
            var historiaBd = await _context.HistoriasMedicas.FindAsync(id);
            if (historiaBd == null) return NotFound("Historia Médica no encontrada.");

            // 🛡️ VALIDACIÓN EXTRA: Si le cambian el dueño a la historia, verificar que el nuevo dueño exista
            if (historiaBd.PacienteId != historiaDTO.PacienteId)
            {
                var pacienteExiste = await _context.Pacientes.AnyAsync(p => p.Id == historiaDTO.PacienteId);
                if (!pacienteExiste) return BadRequest("El nuevo paciente especificado no existe.");
            }

            // SOBREESCRITURA SEGURA
            historiaBd.PacienteId = historiaDTO.PacienteId;
            historiaBd.antecedentesPersonales = (AntecedentesPersonales)historiaDTO.AntecedentesPersonales;
            historiaBd.Farmaco = historiaDTO.Farmaco;
            historiaBd.Dosificacion = historiaDTO.Dosificacion;
            historiaBd.Antidepresivos = historiaDTO.Antidepresivos;
            historiaBd.Ansioliticos = historiaDTO.Ansioliticos;
            historiaBd.Fumador = (Fumador)historiaDTO.Fumador;
            historiaBd.EdadInicio = historiaDTO.EdadInicio;
            historiaBd.ConsumoAlcohol = (ConsumoAlcohol)historiaDTO.ConsumoAlcohol;
            historiaBd.Ejercicio = (Ejercicio)historiaDTO.Ejercicio;
            historiaBd.Traumatismos = historiaDTO.Traumatismos;
            historiaBd.HobbiesDeportes = historiaDTO.HobbiesDeportes;
            historiaBd.Diagnostico = historiaDTO.Diagnostico;
            historiaBd.EscalaDolor = historiaDTO.EscalaDolor;
            
            // Fíjate que ignoramos el EstadoCliente intencionalmente

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUERTA 5: Eliminar Historia Médica (DELETE)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistoriaMedica(int id)
        {
            var historia = await _context.HistoriasMedicas.FindAsync(id);
            if (historia == null) return NotFound();

            _context.HistoriasMedicas.Remove(historia);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}