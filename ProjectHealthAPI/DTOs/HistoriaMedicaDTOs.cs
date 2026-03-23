using System.ComponentModel.DataAnnotations;

namespace ProjectHealthAPI.DTOs
{
    // 🛡️ ENTRADA: Lo que envía Flutter cuando el doctor llena la ficha médica
    public class HistoriaMedicaCreateDTO
    {
        [Required(ErrorMessage = "Debe indicar a qué paciente pertenece esta historia.")]
        public int PacienteId { get; set; }

        // Los Enums los recibimos como números enteros
        public int AntecedentesPersonales { get; set; }
        
        public string Farmaco { get; set; } = string.Empty;
        public string Dosificacion { get; set; } = string.Empty;
        
        public bool Antidepresivos { get; set; }
        
        // Nota: Mantuve tu ortografía exacta ("Ansioloiticos") para que no te dé error con la base de datos, 
        // pero recuerda que la palabra correcta es "Ansiolíticos" por si quieres acomodarlo luego en el Model.
        public bool Ansioliticos { get; set; } 
        
        public int Fumador { get; set; }
        public int EdadInicio { get; set; }
        public int ConsumoAlcohol { get; set; }
        public int Ejercicio { get; set; }
        
        public string Traumatismos { get; set; } = string.Empty;
        public string HobbiesDeportes { get; set; } = string.Empty;
        public string Diagnostico { get; set; } = string.Empty;

        // 🛡️ MUDAMOS EL ESCUDO PARA ACÁ
        [Range(1, 10, ErrorMessage = "La escala de dolor debe ser entre 1 y 10")]
        public int EscalaDolor { get; set; }
        
        // No incluimos EstadoCliente porque nace por defecto en el controlador
    }

    // 📦 SALIDA: Lo que lee Flutter para mostrarle el expediente al doctor
    public class HistoriaMedicaResponseDTO
    {
        public int Id { get; set; }
        public int AntecedentesPersonales { get; set; }
        public string Farmaco { get; set; } = string.Empty;
        public string Dosificacion { get; set; } = string.Empty;
        public bool Antidepresivos { get; set; }
        public bool Ansioliticos { get; set; }
        public int Fumador { get; set; }
        public int EdadInicio { get; set; }
        public int ConsumoAlcohol { get; set; }
        public int Ejercicio { get; set; }
        public string Traumatismos { get; set; } = string.Empty;
        public string HobbiesDeportes { get; set; } = string.Empty;
        public string Diagnostico { get; set; } = string.Empty;
        public int EscalaDolor { get; set; }
        public int EstadoCliente { get; set; }

        // 🪄 EL APLANAMIENTO: Datos clave del Paciente (Para que el doctor sepa de quién es la ficha sin hacer otra consulta)
        public int PacienteId { get; set; }
        public string PacienteNombre { get; set; } = string.Empty;
        public string PacienteCedula { get; set; } = string.Empty;
    }
}