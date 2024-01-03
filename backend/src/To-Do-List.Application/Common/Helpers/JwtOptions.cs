using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace To_Do_List.Application.Common.Helpers;

public class JwtOptions
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
    public int Lifetime { get; set; }

    public SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Key));
}