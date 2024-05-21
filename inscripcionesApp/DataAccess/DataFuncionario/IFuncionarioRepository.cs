using inscripcionesApp.Models;

namespace InscripcionesApp.DataAccess.DataFuncionario
{
    public interface IFuncionarioRepository
    {
        Task<IEnumerable<EstudianteME>> ObtenerEstudiantes();
        Task<EstudianteME> ObtenerEstudiantePorId(int id);
        Task<EstudianteME> ObtenerEstudiantesPorApellidos(string primerApellido, string segundoApellido);
        Task<EstudianteME> ObtenerEstudiantePorIdentificacion(string identificacion);
        Task CrearEstudiante(EstudianteME estudiante, ProgramaME programa);
        // Este metodo(ActualizarRegistroLibro) puede llegar a validar si el libro regresa a biblioteca o sale de esta
        Task ActualizarEstudiante(int id, EstudianteME estudiante);
        Task BorrarEstudiante(int id);
    }
}
