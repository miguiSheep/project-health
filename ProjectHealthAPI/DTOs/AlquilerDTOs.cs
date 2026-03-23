using System.ComponentModel.DataAnnotations;

namespace ProjectHealthAPI.DTOs
{
    // 🛡️ ENTRADA: Lo que envía Flutter para agendar la Alquiler
    public class AlquilerCreateDTO
    {
        [Required(ErrorMessage = "Debe indicar a qué cliente pertenece el Alquiler.")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateOnly Fecha { get; set; }

        [Required(ErrorMessage = "La hora de entrada es obligatoria.")]
        public TimeOnly HoraEntrada { get; set; }

        [Required(ErrorMessage = "La hora de salida es obligatoria.")]
        public TimeOnly HoraSalida { get; set; }
        
        // El EstadoServicio no va, porque toda Alquiler nace en estado 0 (Agendada/Pendiente)
    }

    // 📦 SALIDA: Lo que lee Flutter para mostrar la agenda del día
    public class AlquilerResponseDTO
    {
        public int Id { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly HoraEntrada { get; set; }
        public TimeOnly HoraSalida { get; set; }
        public int EstadoServicio { get; set; }

        // 🪄 EL APLANAMIENTO: Datos clave del Padre (Cliente)
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public string ClienteCedula { get; set; } = string.Empty;
    }
}