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

        [HttpGet]
        public IActionResult ObtenerModalidades(string modalidad)
        {
            var modalidades = _estudianteRepository.ObtenerModalidades();
            return Json(modalidades);
        }


        [HttpGet]
        public IActionResult ObtenerNivelIngreso(string nivelIngreso)
        {
            var nivel = _estudianteRepository.ObtenerNivelIngreso();
            return Json(nivel);
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
