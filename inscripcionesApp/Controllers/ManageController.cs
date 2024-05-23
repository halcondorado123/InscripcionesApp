using inscripcionesApp.Models;
using InscripcionesApp.DataAccess;
using InscripcionesApp.DataAccess.DataFuncionario;
using InscripcionesApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Dapper.SqlMapper;

namespace InscripcionesApp.Controllers
{
    public class ManageController : Controller
    {
        private RepositoryWeb _repositorioWeb;
        public ManageController(RepositoryWeb repositorioWeb)
        {
            _repositorioWeb = repositorioWeb;
        }

        public IActionResult LogIn()
        {
            return View("~/Views/Home/RegistroUsuario.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {

            UsuariosME usuario = _repositorioWeb.LogInUsuario(email, password);
            if (usuario == null)
            {
                ViewData["Mensaje"] = "El usuario ingresado no tiene las credenciales correctas";
                return View();
            }
            else
            {
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                Claim claimUserName = new Claim(ClaimTypes.Name, usuario.Nombre);
                Claim claimRole = new Claim(ClaimTypes.Role, usuario.Tipo);
                Claim claimIdUser = new Claim("IdUsuario", usuario.IdUsuario.ToString());
                Claim claimEmail = new Claim("EmailUsuario", usuario.Email);

                identity.AddClaim(claimUserName);
                identity.AddClaim(claimRole);
                identity.AddClaim(claimIdUser);
                identity.AddClaim(claimEmail);

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.Now.AddMinutes(45)
                });

                // Aquí agregamos un retorno para el caso en que usuario no sea nulo
                return RedirectToAction("PaginaProtegida"); // Cambia "PaginaProtegida" por el nombre de tu acción protegida
            }
        }
        public IActionResult ErrorAcceso()
        {
            ViewData["MENSAJE"] = "Error de acceso";
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<EstudianteME>>> CrearUsuario(string email, string password, string nombre, string apellidos, string tipo)
        {
            try
            {
                var estudiantes = _repositorioWeb.RegistrarUsuario(email, password, nombre, apellidos, tipo);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener los estudiantes: " + ex.Message);
            }
        }

    }
}
