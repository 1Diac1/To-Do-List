using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using To_Do_List.Application.Common.Helpers;
using To_Do_List.Domain.Models;

namespace To_Do_List.Application.Services;

public class JwtService
{
    private readonly JwtOptions _jwtOptions;

    public JwtService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public string GenerateAccessToken(ApplicationUser user)
    {
        var claimsIdentity = GetIdentity(user);

        return this.GenerateToken(claimsIdentity.Claims);
    }
    
    private string GenerateToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(_jwtOptions.Lifetime),
            signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private ClaimsIdentity GetIdentity(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var claimsIdentity = new ClaimsIdentity(claims, "JWT", ClaimTypes.Name, ClaimTypes.Role);

        return claimsIdentity;
    }
}