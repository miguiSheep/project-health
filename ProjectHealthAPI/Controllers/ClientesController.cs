using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHealthAPI.Data;
using ProjectHealthAPI.Models;
using ProjectHealthAPI.DTOs;

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

        // PUERTA 1: Obtener todos los Clientes (GET) - BLINDADA
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteResponseDTO>>> GetClientes()
        {
            var Clientes = await _context.Clientes.ToListAsync();

            // 🪄 TRADUCCIÓN MASIVA: Convertimos la lista de modelos a lista de DTOs
            var respuesta = Clientes.Select(p => new ClienteResponseDTO
            {
                Id = p.Id,
                NombreCompleto = $"{p.Nombre} {p.Apellido}",
                Cedula = p.Cedula,
                Telefono = p.Telefono,
                EstadoCliente = (int)p.EstadoCliente
            }).ToList();

            return Ok(respuesta); // Devolvemos la lista limpia
        }

        // PUERTA 2: Crear un nuevo Cliente (POST) - ¡AHORA CON ESCUDO DTO!
        [HttpPost]
        public async Task<ActionResult<ClienteResponseDTO>> PostCliente(ClienteCreateDTO ClienteDTO)
        {
            // 1. TRADUCCIÓN (Mapeo): Pasamos los datos del escudo al modelo real de la base de datos
            var nuevoCliente = new Cliente
            {
                Nombre = ClienteDTO.Nombre,
                Apellido = ClienteDTO.Apellido,
                Cedula = ClienteDTO.Cedula,
                Genero = (Genero)ClienteDTO.Genero,
                EstadoCivil = (EstadoCivil)ClienteDTO.EstadoCivil,
                Edad = ClienteDTO.Edad,
                Ocupacion = ClienteDTO.Ocupacion,
                Telefono = ClienteDTO.Telefono,
                Direccion = ClienteDTO.Direccion,
                
                // 🛡️ REGLA DE NEGOCIO Y SEGURIDAD: 
                // Como el DTO no trae "EstadoCliente", el hacker no puede manipularlo.
                // Nosotros, como dueños del sistema, decidimos que todo Cliente nuevo nace con Estado 0.
                EstadoCliente = (EstadoCliente)0 
            };

            // 2. GUARDAR EN LA BASE DE DATOS
            _context.Clientes.Add(nuevoCliente);
            await _context.SaveChangesAsync();

            // 3. ARMAR LA RESPUESTA SEGURA: No devolvemos la tabla entera, solo lo que Flutter necesita ver
            var respuesta = new ClienteResponseDTO
            {
                Id = nuevoCliente.Id,
                NombreCompleto = $"{nuevoCliente.Nombre} {nuevoCliente.Apellido}", // Unimos nombre y apellido para comodidad del frontend
                Cedula = nuevoCliente.Cedula,
                Telefono = nuevoCliente.Telefono,
                EstadoCliente = (int)nuevoCliente.EstadoCliente
            };

            // Devolvemos el código 201 Created con el DTO de salida
            return CreatedAtAction(nameof(GetCliente), new { id = nuevoCliente.Id }, respuesta);
        }

        // PUERTA 3: Buscar un Cliente por ID (GET) - BLINDADA
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteResponseDTO>> GetCliente(int id)
        {
            var Cliente = await _context.Clientes.FindAsync(id);

            if (Cliente == null) return NotFound("Cliente no encontrado.");

            // 🪄 TRADUCCIÓN INDIVIDUAL
            var respuesta = new ClienteResponseDTO
            {
                Id = Cliente.Id,
                NombreCompleto = $"{Cliente.Nombre} {Cliente.Apellido}",
                Cedula = Cliente.Cedula,
                Telefono = Cliente.Telefono,
                EstadoCliente = (int)Cliente.EstadoCliente
            };

            return Ok(respuesta);
        }

        // PUERTA 4: Actualizar Cliente (PUT) - BLINDADA
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, ClienteCreateDTO ClienteDTO)
        {
            // 1. Buscamos el registro original
            var ClienteBd = await _context.Clientes.FindAsync(id);
            if (ClienteBd == null) return NotFound();

            // 2. TRADUCCIÓN (Sobreescritura segura)
            // Solo actualizamos los campos que nos interesan, ignorando si el hacker intenta cambiar el Id o Estado
            ClienteBd.Nombre = ClienteDTO.Nombre;
            ClienteBd.Apellido = ClienteDTO.Apellido;
            ClienteBd.Cedula = ClienteDTO.Cedula;
            ClienteBd.Genero = (Genero)ClienteDTO.Genero;
            ClienteBd.EstadoCivil = (EstadoCivil)ClienteDTO.EstadoCivil;
            ClienteBd.Edad = ClienteDTO.Edad;
            ClienteBd.Ocupacion = ClienteDTO.Ocupacion;
            ClienteBd.Telefono = ClienteDTO.Telefono;
            ClienteBd.Direccion = ClienteDTO.Direccion;
            
            // Fíjate que NO tocamos ClienteBd.EstadoCliente. Se queda como estaba.

            // 3. Guardar cambios
            await _context.SaveChangesAsync();

            return NoContent();
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