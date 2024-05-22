using inscripcionesApp.Models;
using InscripcionesApp.Models;

namespace InscripcionesApp.DataAccess.DataEstudiante
{
    public interface IEstudianteRepository
    {
        List<ProgramaME> ObtenerModalidades();
        public List<NivelIngresoME> ObtenerNivelIngreso();
        //List<ProgramaME> ObtenerProgramaInteres(string modalidad, string nivelIngreso);

        //List<ProgramaME> ObtenerEscuela();
        //List<ProgramaME> ObtenerSede();
        //List<ProgramaME> ObtenerPeriodo();
        Task<int> CrearDatosEstudios(ProgramaME programa);
        Task CrearInformacionEstudiante(EstudianteME estudiante, int programaId);
    }
}

