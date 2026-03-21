using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHealthAPI.Data;
using ProjectHealthAPI.Models;
using ProjectHealthAPI.DTOs;

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

        // PUERTA 1: Obtener todos los pacientes (GET) - BLINDADA
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PacienteResponseDTO>>> GetPacientes()
        {
            var pacientes = await _context.Pacientes.ToListAsync();

            // 🪄 TRADUCCIÓN MASIVA: Convertimos la lista de modelos a lista de DTOs
            var respuesta = pacientes.Select(p => new PacienteResponseDTO
            {
                Id = p.Id,
                NombreCompleto = $"{p.Nombre} {p.Apellido}",
                Cedula = p.Cedula,
                Telefono = p.Telefono,
                EstadoCliente = (int)p.EstadoCliente
            }).ToList();

            return Ok(respuesta); // Devolvemos la lista limpia
        }

        // PUERTA 2: Crear un nuevo paciente (POST) - ¡AHORA CON ESCUDO DTO!
        [HttpPost]
        public async Task<ActionResult<PacienteResponseDTO>> PostPaciente(PacienteCreateDTO pacienteDTO)
        {
            // 1. TRADUCCIÓN (Mapeo): Pasamos los datos del escudo al modelo real de la base de datos
            var nuevoPaciente = new Paciente
            {
                Nombre = pacienteDTO.Nombre,
                Apellido = pacienteDTO.Apellido,
                Cedula = pacienteDTO.Cedula,
                Genero = (Genero)pacienteDTO.Genero,
                EstadoCivil = (EstadoCivil)pacienteDTO.EstadoCivil,
                Edad = pacienteDTO.Edad,
                Peso = pacienteDTO.Peso,
                Ocupacion = pacienteDTO.Ocupacion,
                Telefono = pacienteDTO.Telefono,
                Direccion = pacienteDTO.Direccion,
                
                // 🛡️ REGLA DE NEGOCIO Y SEGURIDAD: 
                // Como el DTO no trae "EstadoCliente", el hacker no puede manipularlo.
                // Nosotros, como dueños del sistema, decidimos que todo paciente nuevo nace con Estado 0.
                EstadoCliente = (EstadoCliente)0 
            };

            // 2. GUARDAR EN LA BASE DE DATOS
            _context.Pacientes.Add(nuevoPaciente);
            await _context.SaveChangesAsync();

            // 3. ARMAR LA RESPUESTA SEGURA: No devolvemos la tabla entera, solo lo que Flutter necesita ver
            var respuesta = new PacienteResponseDTO
            {
                Id = nuevoPaciente.Id,
                NombreCompleto = $"{nuevoPaciente.Nombre} {nuevoPaciente.Apellido}", // Unimos nombre y apellido para comodidad del frontend
                Cedula = nuevoPaciente.Cedula,
                Telefono = nuevoPaciente.Telefono,
                EstadoCliente = (int)nuevoPaciente.EstadoCliente
            };

            // Devolvemos el código 201 Created con el DTO de salida
            return CreatedAtAction(nameof(GetPaciente), new { id = nuevoPaciente.Id }, respuesta);
        }

        // PUERTA 3: Buscar un paciente por ID (GET) - BLINDADA
        [HttpGet("{id}")]
        public async Task<ActionResult<PacienteResponseDTO>> GetPaciente(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);

            if (paciente == null) return NotFound("Paciente no encontrado.");

            // 🪄 TRADUCCIÓN INDIVIDUAL
            var respuesta = new PacienteResponseDTO
            {
                Id = paciente.Id,
                NombreCompleto = $"{paciente.Nombre} {paciente.Apellido}",
                Cedula = paciente.Cedula,
                Telefono = paciente.Telefono,
                EstadoCliente = (int)paciente.EstadoCliente
            };

            return Ok(respuesta);
        }

        // PUERTA 4: Actualizar Paciente (PUT) - BLINDADA
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaciente(int id, PacienteCreateDTO pacienteDTO)
        {
            // 1. Buscamos el registro original
            var pacienteBd = await _context.Pacientes.FindAsync(id);
            if (pacienteBd == null) return NotFound();

            // 2. TRADUCCIÓN (Sobreescritura segura)
            // Solo actualizamos los campos que nos interesan, ignorando si el hacker intenta cambiar el Id o Estado
            pacienteBd.Nombre = pacienteDTO.Nombre;
            pacienteBd.Apellido = pacienteDTO.Apellido;
            pacienteBd.Cedula = pacienteDTO.Cedula;
            pacienteBd.Genero = (Genero)pacienteDTO.Genero;
            pacienteBd.EstadoCivil = (EstadoCivil)pacienteDTO.EstadoCivil;
            pacienteBd.Edad = pacienteDTO.Edad;
            pacienteBd.Peso = pacienteDTO.Peso;
            pacienteBd.Ocupacion = pacienteDTO.Ocupacion;
            pacienteBd.Telefono = pacienteDTO.Telefono;
            pacienteBd.Direccion = pacienteDTO.Direccion;
            
            // Fíjate que NO tocamos pacienteBd.EstadoCliente. Se queda como estaba.

            // 3. Guardar cambios
            await _context.SaveChangesAsync();

            return NoContent();
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