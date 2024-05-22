using inscripcionesApp.Models;
using InscripcionesApp.Models;

namespace InscripcionesApp.DataAccess.DataEstudiante
{
    public interface IEstudianteRepository
    {
        IngresosME ObtenerTipoIngreso(string tipoIngreso);
        List<ProgramaME> ObtenerModalidades();
        public List<NivelIngresoME> ObtenerNivelIngreso();
        public List<ProgramaME> ObtenerProgramaInteres(string modalidad, string nivelIngreso);
        public string ObtenerEscuelaPorPrograma(string modalidad, string nivelIngreso, string nombrePrograma);
        public List<ProgramaME> ObtenerPeriodoActivo(string modalidad, string nivelIngreso, string nombrePrograma);
        public List<ProgramaME> ObtenerSede(string modalidad, string nivelIngreso, string nombrePrograma);
        //Task<int> CrearDatosEstudios(ProgramaME programa);
        Task CrearInformacionEstudiante(EstudianteME estudiante, int programaId);
    }
}

