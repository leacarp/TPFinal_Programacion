namespace TPFinal_Programacion.DTOs
{

    // Lo que hacemos con los modelos DTOs es no darle los data anotation y elegimos que queremos que nos retorne el modelo
    public class ClienteDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
    }
}
