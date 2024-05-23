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

        // Información academica del aspirante
        [HttpPost]
        public IActionResult GuardarPrimerFormulario(string tipoIngreso, string modalidad, string nivelIngreso, string programaInteres, string escuela, string periodo, string sede)
        {
            var resultado = _estudianteRepository.CrearDatosEstudios(tipoIngreso, modalidad, nivelIngreso, programaInteres, escuela, periodo, sede);
            if (resultado != null)
            {
                return Json(new { success = true, message = "Datos guardados exitosamente" });
            }
            else
            {
                return Json(new { success = false, message = "Error al guardar los datos" });
            }
        }

        [HttpGet]
        public IActionResult ObtenerSexoEstudiante()
        {
            var sexo = _estudianteRepository.ObtenerSexoEstudiante();
            return Json(sexo);
        }

        //Información academica del aspirante
        [HttpPost]
        public IActionResult GuardarSegundoFormulario(string primerNombre, string segundoNombre, string primerApellido, string segundoApellido, string fechaNacimiento,
           string paisNacimiento, string departamentoNacimiento, string ciudadNacimiento, string direccion, string grupoSanguineo, string tipoDocumento, string numeroDocumento, 
           string fechaExpedicion, string paisExpedicion, string departamentoExpedicion, string ciudadExpedicion, string telefonoPrincipal, string telefonoSecundario,
           string correo, string sexoEstudiante, string estadoCivil)
        {
            var resultado = _estudianteRepository.CrearInformacionEstudiante(primerNombre, segundoNombre, primerApellido, segundoApellido, fechaNacimiento, paisNacimiento, 
                             departamentoNacimiento, ciudadNacimiento, direccion, grupoSanguineo, tipoDocumento, numeroDocumento, fechaExpedicion, paisExpedicion, departamentoExpedicion, 
                             ciudadExpedicion, telefonoPrincipal, telefonoSecundario, correo, sexoEstudiante, estadoCivil);
            if (resultado != null)
            {
                return Json(new { success = true, message = "Datos guardados exitosamente" });
            }
            else
            {
                return Json(new { success = false, message = "Error al guardar los datos" });
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
