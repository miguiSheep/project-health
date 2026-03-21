using System.ComponentModel.DataAnnotations;
namespace ProjectHealthAPI.DTOs
{
    // 🛡️ DTO DE ENTRADA: El escudo para cuando creamos un paciente nuevo
    public class PacienteCreateDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public int Genero { get; set; }
        public int EstadoCivil { get; set; }
        public int Edad { get; set; }
        public int Peso { get; set; }
        public string Ocupacion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        
        // Fíjate que eliminamos el "Id" y el "EstadoCliente". 
        // ¡El usuario ya no puede tocarlos!
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