using Microsoft.AspNetCore.Mvc.Filters;
using To_Do_List.Application.Common.Exceptions;

namespace To_Do_List.Application.Common.Filters;

public class ValidatorActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ModelState.IsValid is false)
            throw new BadRequestException(context.ModelState.Values
                .SelectMany(e => e.Errors)
                .Select(e => e.ErrorMessage));

        await next();
    }
}