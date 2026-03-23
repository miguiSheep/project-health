using System.ComponentModel.DataAnnotations;

namespace ProjectHealthAPI.DTOs
{
    public class PagoCreateDTO
    {
        [Required(ErrorMessage = "El monto es obligatorio.")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "La fecha de pago es obligatoria.")]
        public DateOnly Fecha { get; set; }

        public int TipoServicio { get; set; }
        public int TipoPago { get; set; }
        public int EstadoPago { get; set; }
        
        public string Referencia { get; set; } = string.Empty;
        public string? Comprobante { get; set; } = string.Empty;

        public int? CitaId { get; set; }
        public int? AlquilerId { get; set; }
    }

    public class PagoResponseDTO
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public DateOnly Fecha { get; set; }
        public int TipoServicio { get; set; }
        public int TipoPago { get; set; }
        public int EstadoPago { get; set; }
        public string Referencia { get; set; } = string.Empty;
        public string? Comprobante { get; set; } = string.Empty;

        public int ServicioId { get; set; } 
        public string ResponsableNombre { get; set; } = string.Empty; 
        public string ResponsableDocumento { get; set; } = string.Empty; 
    }
}