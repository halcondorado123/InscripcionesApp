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

        public IActionResult InformacionEstudiante()
        {
            return View("~/Views/Home/InformacionEstudiante.cshtml");
        }

        [HttpGet]
        public IActionResult ObtenerTipoIngreso(string tipoIngreso)
        {
            var tipoIngresoResult = _estudianteRepository.ObtenerTipoIngreso(tipoIngreso);
            return Json(tipoIngresoResult);
        }

        [HttpGet]
        public IActionResult ObtenerModalidades(string modalidad)
        {
            var modalidades = _estudianteRepository.ObtenerModalidades();
            return Json(modalidades);
        }


        [HttpGet]
        public IActionResult ObtenerNivelIngreso()
        {
            var nivelesIngreso = _estudianteRepository.ObtenerNivelIngreso();
            return Json(nivelesIngreso);
        }

        [HttpGet]
        public IActionResult ObtenerProgramaInteres(string modalidad, string nivelIngreso)
        {
            var programasInteres = _estudianteRepository.ObtenerProgramaInteres(modalidad, nivelIngreso);
            return Json(programasInteres);
        }

        [HttpGet]
        public IActionResult ObtenerEscuelaPorPrograma(string modalidad, string nivelIngreso, string nombrePrograma)
        {
            var escuela = _estudianteRepository.ObtenerEscuelaPorPrograma(modalidad, nivelIngreso, nombrePrograma);
            return Json(new { Escuela = escuela });
        }

        [HttpGet]
        public IActionResult ObtenerPeriodo(string modalidad, string nivelIngreso, string nombrePrograma)
        {
            var periodo = _estudianteRepository.ObtenerPeriodoActivo(modalidad, nivelIngreso, nombrePrograma);
            return Json(periodo);
        }

        [HttpGet]
        public IActionResult ObtenerSedePrograma(string modalidad, string nivelIngreso, string nombrePrograma)
        {
            var sede = _estudianteRepository.ObtenerSede(modalidad, nivelIngreso, nombrePrograma);
            return Json(sede);
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
