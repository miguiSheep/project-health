using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHealthAPI.Data;
using ProjectHealthAPI.Models;

namespace ProjectHealthAPI.Controllers
{
    // Esta línea le dice a internet cómo llegar aquí: "tusitio.com/api/Clientes"
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        // "Inyección de dependencias": Le pasamos las llaves de la base de datos al controlador
        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        // PUERTA 1: Obtener todos los Clientes (GET)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            // Va a la base de datos, busca la tabla Clientes y la devuelve como una lista
            return await _context.Clientes.ToListAsync();
        }

        // PUERTA 2: Crear un nuevo Cliente (POST)
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente Cliente)
        {
            // Agrega el Cliente que mandaste a la tabla
            _context.Clientes.Add(Cliente);
            // Guarda los cambios en PostgreSQL
            await _context.SaveChangesAsync();

            // Te devuelve un mensaje de éxito ("Ok") con los datos guardados
            return Ok(Cliente);
        }
        // PUERTA 3: Buscar un Cliente específico por su ID (GET por ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            // Busca en la tabla el Cliente que tenga ese ID exacto
            var Cliente = await _context.Clientes.FindAsync(id);

            // Si no lo encuentra, devuelve un error 404
            if (Cliente == null) 
            {
                return NotFound("Cliente no encontrado en la base de datos.");
            }

            return Cliente;
        }

        // PUERTA 4: Actualizar o Editar un Cliente (PUT)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente Cliente)
        {
            // Medida de seguridad: verificar que el ID de la URL sea el mismo del formulario
            if (id != Cliente.Id) 
            {
                return BadRequest("El ID de la URL no coincide con el del Cliente.");
            }

            // Le avisa a Entity Framework que este registro fue modificado
            _context.Entry(Cliente).State = EntityState.Modified;
            
            // Guarda los cambios en PostgreSQL
            await _context.SaveChangesAsync();

            return NoContent(); // Responde con éxito pero sin devolver texto extra
        }

        // PUERTA 5: Eliminar un Cliente (DELETE)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var Cliente = await _context.Clientes.FindAsync(id);
            if (Cliente == null) 
            {
                return NotFound();
            }

            // Lo borra de la tabla
            _context.Clientes.Remove(Cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}