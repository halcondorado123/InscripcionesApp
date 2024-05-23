using inscripcionesApp.Models;
using InscripcionesApp.Attributes;
using InscripcionesApp.DataAccess;
using InscripcionesApp.DataAccess.DataFuncionario;
using InscripcionesApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InscripcionesApp.Controllers
{
    public class LoginController : Controller
    {
        private RepositoryWeb _repositorioWeb;
        public LoginController(RepositoryWeb repositorio)
        {
            _repositorioWeb = repositorio;
        }

        public IActionResult LoginView()
        {
            return View();
        }

        [AuthorizeUsers]
        public IActionResult PaginaProtegida()
        {
            return View();
        }

        [AuthorizeUsers(Policy = "ADMINISTRADORES")]
        public IActionResult AdminUsuarios() 
        {
            List<UsuariosME> usuarios = this._repositorioWeb.GetUsuarios();
            return View(usuarios);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
