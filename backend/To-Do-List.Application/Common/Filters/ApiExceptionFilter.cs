using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using To_Do_List.Application.Common.Exceptions;

namespace To_Do_List.Application.Common.Filters;

public class ApiExceptionFilter : ExceptionFilterAttribute
{
    private static readonly IDictionary<Type, Action<ExceptionContext>> ExceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            [typeof(BadRequestException)] = HandleBadRequestException,
            [typeof(NotFoundException)] = HandleNotFoundException,
            [typeof(ValidationException)] = HandleFluentValidationException
        };

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);
        
        base.OnException(context);
    }

    private static void HandleException(ExceptionContext context)
    {
        var type = context.Exception.GetType();

        if (ExceptionHandlers.TryGetValue(type, out var handler))
        {
            handler.Invoke(context);
            return;
        }

        if (context.ModelState.IsValid is false)
            return;

        var details = new ValidationProblemDetails(context.ModelState);

        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }

    private static void HandleBadRequestException(ExceptionContext context)
    {
        var exception = (BadRequestException)context.Exception;

        var details = new ProblemDetails
        {
            Title = "Bad request",
        };

        details.Extensions.Add("errors", exception.Errors);

        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }

    private static void HandleNotFoundException(ExceptionContext context)
    {
        var exception = (NotFoundException)context.Exception;

        var details = new ProblemDetails
        {
            Title = "The specified resource was not found",
            Detail = exception.Message
        };

        context.Result = new NotFoundObjectResult(details);
        context.ExceptionHandled = true;
    }

    private static void HandleFluentValidationException(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;
        
        var details = new ProblemDetails
        {
            Title = "Validation failed"
        };
        
        details.Extensions.Add("errors", exception.Errors.Select(e => e.ErrorMessage));
        
        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }
}