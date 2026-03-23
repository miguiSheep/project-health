using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHealthAPI.Data;
using ProjectHealthAPI.Models;
using ProjectHealthAPI.DTOs;

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

        // PUERTA 1: Obtener todas las Alquileres (GET) - BLINDADA Y APLANADA
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlquilerResponseDTO>>> GetAlquileres()
        {
            // 1. Buscamos las Alquileres e INCLUIMOS al papá (Cliente)
            var Alquileres = await _context.Alquileres
                                      .Include(c => c.Cliente) 
                                      .ToListAsync();

            // 2. TRADUCCIÓN MASIVA Y APLANAMIENTO
            var respuesta = Alquileres.Select(c => new AlquilerResponseDTO
            {
                Id = c.Id,
                Fecha = c.Fecha,
                HoraEntrada = c.HoraEntrada,
                HoraSalida = c.HoraSalida,
                EstadoServicio = (int)c.EstadoServicio,
                
                // 🪄 Sacamos los datos del padre para que Flutter no tenga que hacer otra consulta
                ClienteId = c.ClienteId,
                ClienteNombre = c.Cliente != null ? $"{c.Cliente.Nombre} {c.Cliente.Apellido}" : "Desconocido",
                ClienteCedula = c.Cliente != null ? c.Cliente.Cedula : "N/A"
            }).ToList();

            return Ok(respuesta);
        }
        // PUERTA 2: Crear una nueva Alquiler (POST) - BLINDADA
        [HttpPost]
        public async Task<ActionResult<AlquilerResponseDTO>> PostAlquiler(AlquilerCreateDTO AlquilerDTO)
        {
            // 🛡️ VALIDACIÓN EXTRA: Verificamos que el Cliente exista antes de crearle la Alquiler
            var Cliente = await _context.Clientes.FindAsync(AlquilerDTO.ClienteId);
            if (Cliente == null)
            {
                return BadRequest("El Cliente especificado no existe.");
            }

            // 1. TRADUCCIÓN (Mapeo)
            var nuevaAlquiler = new Alquiler
            {
                ClienteId = AlquilerDTO.ClienteId,
                Fecha = AlquilerDTO.Fecha,
                HoraEntrada = AlquilerDTO.HoraEntrada,
                HoraSalida = AlquilerDTO.HoraSalida,
                
                // Forzamos el estado a 0 (Agendada) por seguridad
                EstadoServicio = (EstadoServicio)0 
            };

            // 2. GUARDAR
            _context.Alquileres.Add(nuevaAlquiler);
            await _context.SaveChangesAsync();

            // 3. ARMAR LA RESPUESTA APLANADA
            // Como ya buscamos al Cliente arriba para validarlo, usamos sus datos aquí directamente
            var respuesta = new AlquilerResponseDTO
            {
                Id = nuevaAlquiler.Id,
                Fecha = nuevaAlquiler.Fecha,
                HoraEntrada = nuevaAlquiler.HoraEntrada,
                HoraSalida = nuevaAlquiler.HoraSalida,
                EstadoServicio = (int)nuevaAlquiler.EstadoServicio,
                
                // 🪄 APLANAMIENTO LISTO PARA FLUTTER
                ClienteId = Cliente.Id,
                ClienteNombre = $"{Cliente.Nombre} {Cliente.Apellido}",
                ClienteCedula = Cliente.Cedula
            };

            return CreatedAtAction(nameof(GetAlquiler), new { id = nuevaAlquiler.Id }, respuesta);
        }

        // PUERTA 3: Buscar una Alquiler por ID (GET) - BLINDADA Y APLANADA
        [HttpGet("{id}")]
        public async Task<ActionResult<AlquilerResponseDTO>> GetAlquiler(int id)
        {
            // Buscamos la Alquiler e incluimos los datos del padre
            var Alquiler = await _context.Alquileres
                                     .Include(c => c.Cliente)
                                     .FirstOrDefaultAsync(c => c.Id == id);

            if (Alquiler == null) return NotFound("Alquiler no encontrada.");

            // TRADUCCIÓN INDIVIDUAL
            var respuesta = new AlquilerResponseDTO
            {
                Id = Alquiler.Id,
                Fecha = Alquiler.Fecha,
                HoraEntrada = Alquiler.HoraEntrada,
                HoraSalida = Alquiler.HoraSalida,
                EstadoServicio = (int)Alquiler.EstadoServicio,
                
                ClienteId = Alquiler.ClienteId,
                ClienteNombre = Alquiler.Cliente != null ? $"{Alquiler.Cliente.Nombre} {Alquiler.Cliente.Apellido}" : "Desconocido",
                ClienteCedula = Alquiler.Cliente != null ? Alquiler.Cliente.Cedula : "N/A"
            };

            return Ok(respuesta);
        }

        // PUERTA 4: Actualizar Alquiler (PUT) - BLINDADA
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlquiler(int id, AlquilerCreateDTO AlquilerDTO)
        {
            // 1. Buscamos la Alquiler original
            var AlquilerBd = await _context.Alquileres.FindAsync(id);
            if (AlquilerBd == null) return NotFound("Alquiler no encontrada.");

            // 🛡️ VALIDACIÓN EXTRA: Si el usuario intenta cambiar la Alquiler a otro Cliente, verificamos que el nuevo exista
            if (AlquilerBd.ClienteId != AlquilerDTO.ClienteId)
            {
                var ClienteExiste = await _context.Clientes.AnyAsync(p => p.Id == AlquilerDTO.ClienteId);
                if (!ClienteExiste) return BadRequest("El nuevo Cliente especificado no existe.");
            }

            // 2. TRADUCCIÓN (Sobreescritura segura)
            AlquilerBd.ClienteId = AlquilerDTO.ClienteId;
            AlquilerBd.Fecha = AlquilerDTO.Fecha;
            AlquilerBd.HoraEntrada = AlquilerDTO.HoraEntrada;
            AlquilerBd.HoraSalida = AlquilerDTO.HoraSalida;
            
            // Fíjate que NO tocamos AlquilerBd.EstadoServicio. Eso requeriría otro endpoint o permisos especiales.

            // 3. Guardar cambios
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