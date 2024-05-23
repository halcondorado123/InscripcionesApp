namespace InscripcionesApp.Models
{
    public class EstudianteInscripcionME
    {
        // Validacion dl primer formulario para establecer relacion entre form1 y form2 - Es diferente a ProgramaME, puesto a que almacena la respuesta del estudiante
        public int? Inscripcion_ID {get; set;}       // Vincula con el formulario #02
        public string? Escuela { get; set; }
        public string? Nombre_Programa { get; set; }
        public string? Modalidad { get; set; }
        public string? Nivel_Ingreso { get; set; }
        public string? Periodo { get; set; }
        public string? Sede { get; set; }
    }
}
