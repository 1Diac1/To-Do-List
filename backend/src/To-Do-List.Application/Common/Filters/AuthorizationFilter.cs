using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using To_Do_List.Application.Common.Exceptions;
using To_Do_List.Domain.Models;

namespace To_Do_List.Application.Common.Filters;

public class AuthorizationFilter : IAsyncActionFilter
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthorizationFilter(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var user = await _userManager.GetUserAsync(context.HttpContext.User);

        if (user is null)
            throw new UnauthorizedException();

        await next();
    }
}