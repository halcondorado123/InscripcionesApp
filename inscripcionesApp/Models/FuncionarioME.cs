using InscripcionesApp.Models;

namespace inscripcionesApp.Models
{
    public class FuncionarioME
    {
        public int Func_ID { get; set; }
        public int RolID { get; set; }
        public string? Func_Tipo_Documento { get; set; }
        public string? Func_Numero_Documento { get; set; }
        public string? Func_Nombres_Completos { get; set; }
        public string? Func_Nombres { get; set; }
        public string? Func_Apellidos { get; set; }
        public string? Func_Correo_Corporativo { get; set; }
        public string? Func_Numero_Celular { get; set; }
        public RolME? Rol { get; set; }
    }
}
