using To_Do_List.Application.Common.Filters;
using FluentValidation.AspNetCore;
using To_Do_List.API.Extensions;
using To_Do_List.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using To_Do_List.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.ConfigureJwtAuthentication();
builder.Services.ConfigureGoogleAuthentication();
builder.Services.ConfigureCors();   

builder.Services.Configure<ApiBehaviorOptions>(options =>
    options.SuppressModelStateInvalidFilter = true);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
    options.Filters.Add<ValidatorActionFilter>();
    options.ModelValidatorProviders.Clear();
})
    .AddNewtonsoftJson();

builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();
