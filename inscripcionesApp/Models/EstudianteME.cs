using InscripcionesApp.Models;
using System.Collections.Generic;

namespace inscripcionesApp.Models
{
    public class EstudianteME
    {

        public int Est_ID { get; set; }
        public int RolID { get; set; }
        public int Estatus_Est_ID { get; set; }
        public int Prog_ID { get; set; }
        public int Id_Nivel_Ingreso { get; set; }
        public string? Est_PrimerNombre { get; set; }
        public string? Est_SegundoNombre { get; set; }
        public string? Est_PrimerApellido { get; set; }
        public string? Est_SegundoApellido { get; set; }
        public string? Est_FechaNacimiento { get; set; }
        public string? Est_PaisNacimiento { get; set; }
        public string? Est_DepartamentoNacimiento { get; set; }
        public string? Est_CiudadNacimiento { get; set; }
        public string? Est_Direccion { get; set; }
        public string? Est_GrupoSanguineo { get; set; }
        public string? Est_TipoDocumento { get; set; }
        public string? Est_NumeroDocumento { get; set; }
        public string? Est_FechaExpedicionDoc { get; set; }
        public string? Est_PaisExpedicion { get; set; }
        public string? Est_DepartamentoExpedicion { get; set; }
        public string? Est_CiudadExpedicion { get; set; }
        public string? Est_Sexo { get; set; }
        public string? Est_EstadoCivil { get; set; }
        public string? Est_TelefonoPrincipal { get; set; }
        public string? Est_TelefonoSecundario { get; set; }
        public string? Est_CorreoElectronico { get; set; }
        public string? Est_FechaInscripcion { get; set; }
        public string? Est_FechaActualizacion { get; set; }
        public RolME? Rol { get; set; }
        public EstatusEstudianteME? EstatusEstudiante { get; set; }
        public ProgramaME? Programa { get; set; }
    }
}
