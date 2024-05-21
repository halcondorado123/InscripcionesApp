using inscripcionesApp.Models;

namespace InscripcionesApp.DataAccess.DataEstudiante
{
    public interface IEstudianteRepository
    {
        Task<int>CrearDatosEstudios(ProgramaME programa);
        Task CrearInformacionEstudiante(EstudianteME estudiante, int programaId);
    }
}

