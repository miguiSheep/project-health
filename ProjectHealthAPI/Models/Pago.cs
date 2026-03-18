namespace ProjectHealthAPI.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public decimal Monto { get; set; } //USD
        public TipoServicio TipoServicio { get; set; } //al seleccionar filtrara el tipo de cliente a seleccionar
        public TipoPago TipoPago { get; set; }
        public EstadoPago EstadoPago { get; set; }
        public int? CitaId { get; set; }
        public Cita? Cita { get; set; }
        public int? AlquilerId { get; set; }
        public Alquiler? Alquiler { get; set; }
        public int Referencia { get; set; }
        public String Comprobante { get; set; } = string.Empty;
    }
}