using System.ComponentModel.DataAnnotations;

namespace ProjectHealthAPI.DTOs
{
    // 🛡️ ENTRADA: Lo que envía Flutter para agendar la cita
    public class CitaCreateDTO
    {
        [Required(ErrorMessage = "Debe indicar a qué paciente pertenece la cita.")]
        public int PacienteId { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateOnly Fecha { get; set; }

        [Required(ErrorMessage = "La hora de entrada es obligatoria.")]
        public TimeOnly HoraEntrada { get; set; }

        [Required(ErrorMessage = "La hora de salida es obligatoria.")]
        public TimeOnly HoraSalida { get; set; }
        
        // El EstadoServicio no va, porque toda cita nace en estado 0 (Agendada/Pendiente)
    }

    // 📦 SALIDA: Lo que lee Flutter para mostrar la agenda del día
    public class CitaResponseDTO
    {
        public int Id { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly HoraEntrada { get; set; }
        public TimeOnly HoraSalida { get; set; }
        public int EstadoServicio { get; set; }

        // 🪄 EL APLANAMIENTO: Datos clave del Padre (Paciente)
        public int PacienteId { get; set; }
        public string PacienteNombre { get; set; } = string.Empty;
        public string PacienteCedula { get; set; } = string.Empty;
    }
}