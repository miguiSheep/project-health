using System.ComponentModel.DataAnnotations;
namespace ProjectHealthAPI.DTOs
{
    // 🛡️ DTO DE ENTRADA: El escudo para cuando creamos un paciente nuevo
    public class PacienteCreateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 letras.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellido { get; set; } = string.Empty;

        // 🛡️ REGLA FLEXIBLE PARA CÉDULAS/PASAPORTES
        // Acepta V- o E- seguido de 6 a 8 números, O acepta letras y números para pasaportes extranjeros.
        [Required(ErrorMessage = "El documento de identidad es obligatorio.")]
        [RegularExpression(@"^([VE]-[0-9]{6,8}|[a-zA-Z0-9]{5,15})$", ErrorMessage = "Formato inválido. Use V-12345678 o un pasaporte válido sin caracteres especiales.")]
        public string Cedula { get; set; } = string.Empty;

        [Range(0, 120, ErrorMessage = "La edad debe estar entre 0 y 120 años.")]
        public int Edad { get; set; }

        public int Genero { get; set; }
        public int EstadoCivil { get; set; }
        public int Peso { get; set; }
        public string Ocupacion { get; set; } = string.Empty;

        // 🛡️ REGLA FLEXIBLE PARA TELÉFONOS
        // Acepta números que pueden empezar con un '+' opcional, seguidos de 10 a 15 dígitos.
        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "El teléfono debe tener entre 10 y 15 números continuos. Puede incluir el símbolo + al inicio.")]
        public string Telefono { get; set; } = string.Empty;

        public string Direccion { get; set; } = string.Empty;
    }

    // 📦 DTO DE SALIDA: Lo que le enviamos a Flutter para que lo lea
    public class PacienteResponseDTO
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public int EstadoCliente { get; set; }
    }
}