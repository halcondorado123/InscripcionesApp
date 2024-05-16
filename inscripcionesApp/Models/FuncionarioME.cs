namespace inscripcionesApp.Models
{
    public class FuncionarioME : PersonaME
    {
        public int? id { get; set; }
        public string? Rol { get; set; }
        public string? User { get; set;}
        public string? Password { get; set;}
    }
}
