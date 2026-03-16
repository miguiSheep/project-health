namespace ProjectHealthAPI.Models
{
    public class Servicio
    {
        public int Id { get; set; }
        public TipoServicio TipoServicio { get; set; }
        public String Cliente { get; set; } = string.Empty;
        public DateOnly Fecha { get; set; }
        public TimeOnly HoraEntrada { get; set; }
        public TimeOnly HoraSalida { get; set; }
        public EstadoServicio EstadoServicio { get; set; }
    }
}