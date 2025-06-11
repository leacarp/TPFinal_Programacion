namespace TPFinal_Programacion.DTOs
{
    public class MovimientoPostDTO
    {
        public string CryptoCode { get; set; }
        public string Action {  get; set; }
        public decimal CryptoAmount { get; set; }
        public DateTime DateTime { get; set; }
        public int ClienteId { get; set; }
    }
}
