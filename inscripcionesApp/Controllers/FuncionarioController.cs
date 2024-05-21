using InscripcionesApp.DataAccess.DataEstudiante;
using inscripcionesApp.Models;
using Microsoft.AspNetCore.Mvc;
using InscripcionesApp.DataAccess.DataFuncionario;
using System.Linq;

namespace InscripcionesApp.Controllers
{
    public class FuncionarioController : Controller
    {
        private readonly IFuncionarioRepository _funcionarioRepository;

        public FuncionarioController(IFuncionarioRepository funcionarioRepository)
        {
            _funcionarioRepository = funcionarioRepository;
        }

        //public IActionResult Registrate()
        //{
        //    return View("~/Views/Home/Registrate.cshtml");
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstudianteME>>> ObtenerEstudiantes()
        {
            try
            {
                var estudiantes = await _funcionarioRepository.ObtenerEstudiantes();
                return Ok(estudiantes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener los estudiantes: " + ex.Message);
            }
        }

        [HttpGet("id/{id:int}")]
        public async Task<ActionResult<EstudianteME>> ObtenerEstudiantePorId(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID inválido.");
            }

            try
            {
                var estudiante = await _funcionarioRepository.ObtenerEstudiantePorId(id);
                if (estudiante == null)
                {
                    return NotFound("Estudiante no encontrado");
                }
                return Ok(estudiante);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener el estudiante: " + ex.Message);
            }
        }

        [HttpGet]
        //[HttpGet("{primerApellido}/{segundoApellido}")]
        public async Task<ActionResult<IEnumerable<EstudianteME>>> ObtenerEstudiantesPorApellidos(string primerApellido, string segundoApellido)
        {
            try
            {
                var estudiantes = await _funcionarioRepository.ObtenerEstudiantesPorApellidos(primerApellido, segundoApellido);
                if (estudiantes == null)
                {
                    return NotFound("No se encontraron estudiantes con los apellidos proporcionados");
                }
                return Ok(estudiantes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener los estudiantes por apellidos: " + ex.Message);
            }
        }


        [HttpGet("{identificacion}")]
        public async Task<ActionResult<IEnumerable<EstudianteME>>> ObtenerEstudiantePorIdentificacion(string identificacion)
        {
            try
            {
                var estudiantes = await _funcionarioRepository.ObtenerEstudiantePorIdentificacion(identificacion);
                if (estudiantes == null)
                {
                    return NotFound("No se encontraron estudiantes con el documento proporcionado");
                }
                return Ok(estudiantes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener los estudiantes: " + ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> CrearEstudiante([FromBody] ProgramaME programa, EstudianteME estudiante)
        {
            try
            {
                // Aquí deberías llamar al método para crear un nuevo estudiante en tu repositorio
                await _funcionarioRepository.CrearEstudiante(estudiante, programa);

                // Si el estudiante se crea correctamente, puedes devolver una respuesta 200 OK
                return Ok("Estudiante creado correctamente");
            }
            catch (Exception ex)
            {
                // Si ocurre un error, devolver un código de estado 500 y el mensaje de error
                return StatusCode(500, "Error al crear el estudiante: " + ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarEstudiante(int id, [FromBody] EstudianteME estudiante)
        {
            try
            {
                await _funcionarioRepository.ActualizarEstudiante(id, estudiante);
                return Ok("Estudiante actualizado correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al actualizar el estudiante: " + ex.Message);
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> BorrarEstudiantePorId(int id)
        {
            try
            {
                await _funcionarioRepository.BorrarEstudiante(id);
                return Ok("Estudiante eliminado correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al eliminar el estudiante: " + ex.Message);
            }
        }

    }
}
