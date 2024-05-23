using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InscripcionesApp.Attributes
{
    public class AuthorizeUsersAttribute : TypeFilterAttribute
    {
        public AuthorizeUsersAttribute() : base(typeof(AuthorizeUsersFilter))
        {
        }

        public string Policy { get; set; }
    }
}