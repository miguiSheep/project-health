using System.ComponentModel.DataAnnotations;

namespace ProjectHealthAPI.Models
{
    public class HistoriaMedica
    {
        public int Id { get; set; }
        public String Paciente { get; set; } = string.Empty;
        public AntecedentesPersonales antecedentesPersonales { get; set; }
        public String Framaco { get; set; } = string.Empty;
        public String Dosificacion { get; set; } = string.Empty;
        public Boolean Antidepresivos { get; set; }
        public Boolean Ansioloiticos { get; set; }
        public Fumador Fumador { get; set; }
        public int EdadInicio { get; set; } 
        public ConsumoAlcohol ConsumoAlcohol { get; set; }
        public Ejercicio Ejercicio { get; set; }
        public EstadoCliente EstadoCliente { get; set; }
        public String Traumatismos { get; set; } = string.Empty;
        public String HobbiesDeportes { get; set; } = string.Empty;
        public String Diagnostico { get; set; } = string.Empty;
        [Range(1, 10, ErrorMessage = "La escala de dolor debe ser entre  1 y 10")]

        public int EscalaDolor { get; set; }
    }
}