using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using To_Do_List.API.Contracts;
using To_Do_List.Application.Common.Exceptions;

namespace To_Do_List.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("google-login")]
    public IActionResult GoogleLogin()
    {
        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(GoogleResponse)),
            Items =
            {
                {"scheme", GoogleDefaults.AuthenticationScheme}
            }
        };

        return Challenge(authenticationProperties, GoogleDefaults.AuthenticationScheme);
    }

    [AllowAnonymous]
    [HttpGet("google-response")]
    public async Task<IActionResult> GoogleResponse()
    {
        var authenticationResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        if (authenticationResult.Succeeded is false)
            throw new BadRequestException(ErrorMessages.FailedAuthentication);

        var claims = authenticationResult.Principal.Identities
            .FirstOrDefault()?
            .Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });

        return Ok(claims);
    }
}