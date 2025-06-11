using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPFinal_Programacion.Models
{
    public class Movimiento
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "ERROR - Ingrese una crypto valida")]
        public string CryptoCode {  get; set; }
        [Required(ErrorMessage = "ERROR - Ingrese una acción valida (compra o venta)")]
        public string Action {  get; set; }
        [Required(ErrorMessage = "ERROR - Ingrese una cantidad valida")]
        [Column(TypeName = "decimal(18,8)")]
        public decimal CryptoAmount { get; set; }
        public decimal Pesos {  get; set; }
        public DateTime DateTime { get; set; }
        public int ClienteId { get; set; }
        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; }
    }
}
