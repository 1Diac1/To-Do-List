using To_Do_List.Application.Common.Exceptions;
using To_Do_List.Application.Common.Filters;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace To_Do_List.Application.UnitTests.Filters;

public class ApiExceptionFilterTests
{
    [Fact]
    public void ExceptionHandler_ShouldHandleBadRequestException()
    {
        // arrange
        var exceptionFilter = new ApiExceptionFilter();
        var exceptionContext = new ExceptionContext(
            new ActionContext()
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            },
            new List<IFilterMetadata>()) { Exception = new BadRequestException("Something bad request exception message") };
        
        // act
        exceptionFilter.OnException(exceptionContext);
        
        // assert
        Assert.True(exceptionContext.ExceptionHandled);
        Assert.IsType<BadRequestObjectResult>(exceptionContext.Result);
        Assert.Equal("Something bad request exception message", exceptionContext.Exception.Message);
    }

    [Fact]
    public void ExceptionHandler_ShouldHandleUnauthorizedException()
    {
        // arrange
        var exceptionFilter = new ApiExceptionFilter();
        var exceptionContext = new ExceptionContext(
            new ActionContext()
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            },
            new List<IFilterMetadata>()) { Exception = new UnauthorizedException() };
        
        // act
        exceptionFilter.OnException(exceptionContext);
        
        // assert
        Assert.True(exceptionContext.ExceptionHandled);
        Assert.IsType<UnauthorizedObjectResult>(exceptionContext.Result);
    }
}