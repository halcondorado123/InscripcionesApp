using inscripcionesApp.Models;
using InscripcionesApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace InscripcionesApp.DataAccess.DataEstudiante
{
    public interface IEstudianteRepository
    {
        IngresosME ObtenerTipoIngreso(string tipoIngreso);
        List<ProgramaME> ObtenerModalidades();
        List<NivelIngresoME> ObtenerNivelIngreso();
        List<ProgramaME> ObtenerProgramaInteres(string modalidad, string nivelIngreso);
        string ObtenerEscuelaPorPrograma(string modalidad, string nivelIngreso, string nombrePrograma);
        List<ProgramaME> ObtenerPeriodoActivo(string modalidad, string nivelIngreso, string nombrePrograma);
         List<ProgramaME> ObtenerSede(string modalidad, string nivelIngreso, string nombrePrograma);
        Task<EstudianteInscripcionME> CrearDatosEstudios(string tipoIngreso, string modalidad, string nivelIngreso, string programaInteres, string escuela, string periodo, string sede);
        Task<List<SexoEstudianteME>> ObtenerSexoEstudiante();
        public Task CrearInformacionEstudiante(string primerNombre, string segundoNombre, string primerApellido, string segundoApellido, string fechaNacimiento,
           string paisNacimiento, string departamentoNacimiento, string direccion, string ciudadNacimiento, string grupoSanguineo, string tipoDocumento, string numeroDocumento,
           string fechaExpedicion, string paisExpedicion, string departamentoExpedicion, string ciudadExpedicion, string telefonoPrincipal, string telefonoSecundario,
           string correo, string sexoEstudiante, string estadoCivil);
    }
}

