using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using To_Do_List.Application.Interfaces;
using To_Do_List.Domain.Models;
using To_Do_List.Infrastructure.Data;

namespace To_Do_List.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IApplicationDbContext), typeof(ApplicationDbContext));

        // TODO: переместить в appsettings.json
        services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite("Data Source=To-Do-List.db"));

        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        return services;
    }
}