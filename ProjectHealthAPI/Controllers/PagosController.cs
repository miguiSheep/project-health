using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHealthAPI.Data;
using ProjectHealthAPI.Models;
using ProjectHealthAPI.DTOs;

namespace ProjectHealthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class PagosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PagosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PagoResponseDTO>>> GetPagos()
        {
            var pagos = await _context.Pagos
                                      .Include(p => p.Cita)
                                          .ThenInclude(c => c!.Paciente)
                                      .Include(p => p.Alquiler)
                                          .ThenInclude(a => a!.Cliente)
                                      .ToListAsync();

            var respuesta = pagos.Select(p => new PagoResponseDTO
            {
                Id = p.Id,
                Monto = p.Monto,
                Fecha = p.Fecha, 
                TipoServicio = (int)p.TipoServicio,
                TipoPago = (int)p.TipoPago,
                EstadoPago = (int)p.EstadoPago,
                Referencia = p.Referencia,
                Comprobante = p.Comprobante,
                
                ServicioId = p.TipoServicio == TipoServicio.Cita ? p.CitaId ?? 0 : p.AlquilerId ?? 0,
                
                ResponsableNombre = p.TipoServicio == TipoServicio.Cita 
                    ? (p.Cita?.Paciente != null ? $"{p.Cita.Paciente.Nombre} {p.Cita.Paciente.Apellido}" : "Desconocido") 
                    : (p.Alquiler?.Cliente != null ? $"{p.Alquiler.Cliente.Nombre} {p.Alquiler.Cliente.Apellido}" : "Desconocido"),
                    
                ResponsableDocumento = p.TipoServicio == TipoServicio.Cita 
                    ? (p.Cita?.Paciente != null ? p.Cita.Paciente.Cedula : "N/A") 
                    : (p.Alquiler?.Cliente != null ? p.Alquiler.Cliente.Cedula : "N/A")
            }).ToList();

            return Ok(respuesta);
        }

        [HttpPost]
        public async Task<ActionResult<PagoResponseDTO>> PostPago(PagoCreateDTO pagoDTO)
        {
            if (pagoDTO.TipoServicio == (int)TipoServicio.Cita && pagoDTO.CitaId == null)
            {
                return BadRequest("Error: Si el pago es por una Cita, debes proporcionar el CitaId.");
            }

            if (pagoDTO.TipoServicio == (int)TipoServicio.Alquiler && pagoDTO.AlquilerId == null)
            {
                return BadRequest("Error: Si el pago es por un Alquiler, debes proporcionar el AlquilerId.");
            }

            var nuevoPago = new Pago
            {
                Monto = pagoDTO.Monto,
                Fecha = pagoDTO.Fecha, 
                TipoServicio = (TipoServicio)pagoDTO.TipoServicio,
                TipoPago = (TipoPago)pagoDTO.TipoPago,
                EstadoPago = (EstadoPago)pagoDTO.EstadoPago,
                Referencia = pagoDTO.Referencia,
                Comprobante = pagoDTO.Comprobante,
                CitaId = pagoDTO.CitaId,
                AlquilerId = pagoDTO.AlquilerId
            };

            _context.Pagos.Add(nuevoPago);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPago), new { id = nuevoPago.Id }, null);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PagoResponseDTO>> GetPago(int id)
        {
            var p = await _context.Pagos
                                  .Include(x => x.Cita)
                                      .ThenInclude(c => c!.Paciente)
                                  .Include(x => x.Alquiler)
                                      .ThenInclude(a => a!.Cliente)
                                  .FirstOrDefaultAsync(x => x.Id == id);

            if (p == null) return NotFound("No existe un pago con ese ID.");

            var respuesta = new PagoResponseDTO
            {
                Id = p.Id,
                Monto = p.Monto,
                Fecha = p.Fecha,
                TipoServicio = (int)p.TipoServicio,
                TipoPago = (int)p.TipoPago,
                EstadoPago = (int)p.EstadoPago,
                Referencia = p.Referencia,
                Comprobante = p.Comprobante,
                
                ServicioId = p.TipoServicio == TipoServicio.Cita ? p.CitaId ?? 0 : p.AlquilerId ?? 0,
                
                ResponsableNombre = p.TipoServicio == TipoServicio.Cita 
                    ? (p.Cita?.Paciente != null ? $"{p.Cita.Paciente.Nombre} {p.Cita.Paciente.Apellido}" : "Desconocido") 
                    : (p.Alquiler?.Cliente != null ? $"{p.Alquiler.Cliente.Nombre} {p.Alquiler.Cliente.Apellido}" : "Desconocido"),
                    
                ResponsableDocumento = p.TipoServicio == TipoServicio.Cita 
                    ? (p.Cita?.Paciente != null ? p.Cita.Paciente.Cedula : "N/A") 
                    : (p.Alquiler?.Cliente != null ? p.Alquiler.Cliente.Cedula : "N/A")
            };

            return Ok(respuesta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPago(int id, PagoCreateDTO pagoDTO)
        {
            var pagoBd = await _context.Pagos.FindAsync(id);
            if (pagoBd == null) return NotFound();

            pagoBd.Monto = pagoDTO.Monto;
            pagoBd.Fecha = pagoDTO.Fecha;
            pagoBd.TipoServicio = (TipoServicio)pagoDTO.TipoServicio;
            pagoBd.TipoPago = (TipoPago)pagoDTO.TipoPago;
            pagoBd.EstadoPago = (EstadoPago)pagoDTO.EstadoPago;
            pagoBd.Referencia = pagoDTO.Referencia;
            pagoBd.Comprobante = pagoDTO.Comprobante;
            pagoBd.CitaId = pagoDTO.CitaId;
            pagoBd.AlquilerId = pagoDTO.AlquilerId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePago(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null) return NotFound();

            _context.Pagos.Remove(pago);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}