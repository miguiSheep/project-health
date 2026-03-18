namespace ProjectHealthAPI.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public String Nombre { get; set; } = string.Empty;
        public String Apellido { get; set; } = string.Empty;
        public String Cedula { get; set; } = string.Empty;
        public Genero Genero { get; set; }
        public EstadoCivil EstadoCivil { get; set; }
        public int Edad { get; set; }
        public int Peso { get; set; }
        public EstadoCliente EstadoCliente { get; set; }
        public String Ocupacion { get; set; } = string.Empty;
        public String Telefono { get; set; } = string.Empty;
        public String Direccion { get; set; } = string.Empty;
    }
}