using System.ComponentModel.DataAnnotations;

namespace TPFinal_Programacion.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "ERROR - Ingrese un nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "ERROR - Ingrese un email")]
        public string Email { get; set; }
    }
}
