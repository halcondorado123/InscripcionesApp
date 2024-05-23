using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

public class AuthorizeUsersFilter : IAsyncAuthorizationFilter
{
    private readonly IAuthorizationService _authorizationService;

    public AuthorizeUsersFilter(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (!user.Identity.IsAuthenticated)
        {
            context.Result = new ForbidResult();
            return;
        }

        var isAuthorized = await _authorizationService.AuthorizeAsync(user, "UserPolicy");
        if (!isAuthorized.Succeeded)
        {
            context.Result = new ForbidResult();
        }
    }
}