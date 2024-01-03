using System.Text;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace To_Do_List.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureJwtAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
            
                    ValidIssuer = "https://localhost:5000",
                    ValidAudience = "https://localhost:5000",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SomethingKey"))
                };
            });

        services.AddAuthorization();
        
        return services;
    }

    public static IServiceCollection ConfigureGoogleAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        })
            .AddGoogle(options =>
            {
                options.ClientId = "219229931408-0cj4jdphc9dnt3nhggpvkgfudrl5f7up.apps.googleusercontent.com";
                options.ClientSecret = "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";
            });
        
        return services;
    }

    public static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
        
        return services;
    }
}