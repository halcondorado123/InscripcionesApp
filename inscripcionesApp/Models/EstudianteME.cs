namespace inscripcionesApp.Models
{
    public class EstudianteME : PersonaME
    {
        public int? id { get; set; }
        public string? Usuario { get; set; }
        public string? Estatus { get; set; }    // Tipo de aspirante
        public DateTime? FechaInscripcion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

    }
}
