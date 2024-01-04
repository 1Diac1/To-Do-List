using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace To_Do_List.Application.Common.Helpers;

public class JwtOptions
{
    public virtual string Issuer { get; set; }
    public virtual string Audience { get; set; }
    public virtual string Key { get; set; }
    public virtual int Lifetime { get; set; }

    public SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Key));

    public JwtOptions(string issuer, string audience, string key, int lifetime)
    {
        Issuer = issuer;
        Audience = audience;
        Key = key;
        Lifetime = lifetime;
    }
}