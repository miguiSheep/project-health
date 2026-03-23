namespace ProjectHealthAPI.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public decimal Monto { get; set; } 
        public DateOnly Fecha { get; set; } // Propiedad añadida para el control financiero
        public TipoServicio TipoServicio { get; set; } 
        public TipoPago TipoPago { get; set; }
        public EstadoPago EstadoPago { get; set; }
        public int? CitaId { get; set; }
        public Cita? Cita { get; set; }
        public int? AlquilerId { get; set; }
        public Alquiler? Alquiler { get; set; }
        public string Referencia { get; set; } = string.Empty; // Modificado para evitar pérdida de datos
        public string? Comprobante { get; set; } = string.Empty;
    }
}