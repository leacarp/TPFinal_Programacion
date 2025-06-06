using System.ComponentModel.DataAnnotations;

namespace TPFinal_Programacion.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "ERROR - Ingrese un nombre válido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "ERROR - Ingrese un email válido")]
        [EmailAddress(ErrorMessage = "ERROR - El formato del email es inválido")]
        public string Email { get; set; }
    }
}
