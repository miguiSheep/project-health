namespace ProjectHealthAPI.Models
{
    public class Alquiler
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly HoraEntrada { get; set; }
        public TimeOnly HoraSalida { get; set; }
        public EstadoServicio EstadoServicio { get; set; }
    }
}