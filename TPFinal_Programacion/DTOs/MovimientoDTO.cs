namespace TPFinal_Programacion.DTOs
{
    public class MovimientoDTO
    {
        public int Id { get; set; }
        public string CryptoCode { get; set; }
        public string Action { get; set; }
        public decimal CryptoAmount {  get; set; }
        public decimal Pesos {  get; set; }
        public DateTime DateTime { get; set; }
        public string Cliente { get; set; }
    }
}
