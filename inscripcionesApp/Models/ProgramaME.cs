
using InscripcionesApp.Models;

namespace inscripcionesApp.Models
{
    public class ProgramaME
    {
        public string? Prog_ID { get; set; }
        public string? NivelIngreso { get; set; }
        public string? Escuela { get; set; }
        public string? Nombre_Programa { get; set; }
        public string? Modalidad { get; set; }
        public string? Sede { get; set; }
        public string? Periodo { get; set; }
        public IngresosME? Ingreso { get; set; }
    }
}
