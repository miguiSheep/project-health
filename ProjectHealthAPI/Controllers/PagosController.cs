using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHealthAPI.Data;
using ProjectHealthAPI.Models;

namespace ProjectHealthAPI.Controllers
{
    // 📍 ENRUTAMIENTO: Define que la URL para llegar aquí será "api/Pagos"
    [Route("api/[controller]")]
    [ApiController] 
    public class PagosController : ControllerBase
    {
        // Variable privada que guardará la conexión a la base de datos
        private readonly AppDbContext _context;

        // 💉 INYECCIÓN DE DEPENDENCIAS: El constructor recibe la conexión y la guarda
        public PagosController(AppDbContext context)
        {
            _context = context;
        }

        // ====================================================================
        // 🚪 PUERTA 1: LEER TODOS LOS PAGOS (GET)
        // ====================================================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pago>>> GetPagos()
        {
            // Va a la tabla Pagos.
            // .Include(p => p.Cita) -> Intenta adjuntar los datos de la cita (si existe).
            // .Include(p => p.Alquiler) -> Intenta adjuntar los datos del alquiler (si existe).
            return await _context.Pagos
                                 .Include(p => p.Cita)
                                 .Include(p => p.Alquiler)
                                 .ToListAsync();
        }

        // ====================================================================
        // 🚪 PUERTA 2: CREAR UN NUEVO PAGO (POST)
        // ====================================================================
        [HttpPost]
        public async Task<ActionResult<Pago>> PostPago(Pago pago)
        {
            // 🛡️ REGLA DE NEGOCIO 1: Validación de Citas
            // "Si en el Enum marcaste 'Cita' (valor 1), pero el CitaId viene nulo (vacío)..."
            if (pago.TipoServicio == TipoServicio.Cita && pago.CitaId == null)
            {
                // BadRequest devuelve un Error 400 (Culpa del usuario por mandar datos incompletos)
                return BadRequest("Error: Si el pago es por una Cita, debes proporcionar el CitaId.");
            }

            // 🛡️ REGLA DE NEGOCIO 2: Validación de Alquileres
            // "Si en el Enum marcaste 'Alquiler' (valor 0), pero el AlquilerId viene nulo..."
            if (pago.TipoServicio == TipoServicio.Alquiler && pago.AlquilerId == null)
            {
                return BadRequest("Error: Si el pago es por un Alquiler, debes proporcionar el AlquilerId.");
            }

            // Si pasa la policía, Entity Framework prepara el registro
            _context.Pagos.Add(pago);
            // Salva los cambios físicamente en PostgreSQL
            await _context.SaveChangesAsync();

            // Devuelve un código 200 OK y muestra cómo quedó guardado el pago
            return Ok(pago);
        }

        // ====================================================================
        // 🚪 PUERTA 3: LEER UN PAGO ESPECÍFICO (GET POR ID)
        // ====================================================================
        [HttpGet("{id}")]
        public async Task<ActionResult<Pago>> GetPago(int id)
        {
            // FirstOrDefaultAsync: Busca el primer pago cuyo Id coincida con el número de la URL
            var pago = await _context.Pagos
                                     .Include(p => p.Cita)
                                        .ThenInclude(c => c.Paciente)
                                     .Include(p => p.Alquiler)
                                        .ThenInclude(c => c.Cliente)
                                     .FirstOrDefaultAsync(p => p.Id == id);

            // Si no consigue nada, devuelve Error 404 (No Encontrado)
            if (pago == null) 
            {
                return NotFound("No existe un pago con ese ID.");
            }

            return pago;
        }

        // ====================================================================
        // 🚪 PUERTA 4: ACTUALIZAR UN PAGO (PUT)
        // ====================================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPago(int id, Pago pago)
        {
            // Medida de seguridad: Evita que alguien intente editar el pago 5 usando la URL del pago 2
            if (id != pago.Id) return BadRequest();

            // Le avisa al motor que este registro fue modificado y debe sobreescribirse
            _context.Entry(pago).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Devuelve 204 No Content (Éxito, pero no hay texto que mostrar)
            return NoContent();
        }

        // ====================================================================
        // 🚪 PUERTA 5: ELIMINAR UN PAGO (DELETE)
        // ====================================================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePago(int id)
        {
            // Primero busca si el pago existe
            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null) return NotFound();

            // Lo marca para eliminación
            _context.Pagos.Remove(pago);
            // Ejecuta la orden en la base de datos
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}