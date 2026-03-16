namespace ProjectHealthAPI.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public float Monto { get; set; } //USD
        public TipoServicio TipoServicio { get; set; } //al seleccionar filtrara el tipo de cliente a seleccionar
        public TipoPago TipoPago { get; set; }
        public EstadoPago EstadoPago { get; set; }
        public String Cliente { get; set; } = string.Empty;
        public int Referencia { get; set; }
        public String Comprobante { get; set; } = string.Empty;
    }
}