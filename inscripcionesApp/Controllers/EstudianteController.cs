using inscripcionesApp.Models;
using InscripcionesApp.DataAccess.DataEstudiante;
using InscripcionesApp.DataAccess.DataFuncionario;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace InscripcionesApp.Controllers
{
    public class EstudianteController : Controller
    {
        private readonly IEstudianteRepository _estudianteRepository;

        public EstudianteController(IEstudianteRepository estudianteRepository)
        {
            _estudianteRepository = estudianteRepository;
        }

        public IActionResult Registrate()
        {
            return View("~/Views/Home/Registrate.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> CrearDatosEstudios([FromBody] ProgramaME programa, EstudianteME estudiante)
        {
            HttpContext.Session.SetObject("Estudiante", estudiante);

            try
            {
                // Aquí deberías llamar al método para crear un nuevo estudiante en tu repositorio
                await _estudianteRepository.CrearDatosEstudios(programa);

                // Si el estudiante se crea correctamente, puedes devolver una respuesta 200 OK
                return Ok("Estudiante creado correctamente");
            }
            catch (Exception ex)
            {
                // Si ocurre un error, devolver un código de estado 500 y el mensaje de error
                return StatusCode(500, "Error al crear el estudiante: " + ex.Message);
            }
        }
    }
        public static class SessionExtensions
        {
            public static void SetObject<T>(this ISession session, string key, T value)
            {
                session.SetString(key, JsonSerializer.Serialize(value));
            }

            public static T GetObject<T>(this ISession session, string key)
            {
                var value = session.GetString(key);
                return value == null ? default : JsonSerializer.Deserialize<T>(value);
            }
        }
}
