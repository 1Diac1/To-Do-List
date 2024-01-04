using To_Do_List.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc.Abstractions;
using To_Do_List.Application.Common.Filters;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace To_Do_List.Application.UnitTests.Filters;

public class ValidatorActionFilterTests
{
    private readonly ActionExecutingContext _actionExecutingContext;

    public ValidatorActionFilterTests()
    {
        var httpContext = new DefaultHttpContext();
        _actionExecutingContext = new ActionExecutingContext(new ActionContext
            {
                HttpContext = httpContext,
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            },
            new List<IFilterMetadata>(),
            new Dictionary<string, object>(),
            Mock.Of<ControllerBase>()); 
    }
    
    [Fact]
    public async Task OnActionExecutionAsync_ShouldThrowBadRequestException_WhenModelStateIsNotValid()
    {
        // arrange
        _actionExecutingContext.ModelState.AddModelError("Something key", "Something error message");
        var validatorActionFilter = new ValidatorActionFilter();
        
        // assert & act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => validatorActionFilter.OnActionExecutionAsync(_actionExecutingContext, () =>
                Task.FromResult(new ActionExecutedContext(_actionExecutingContext, new List<IFilterMetadata>(), new object()))));
        Assert.Contains("Something error message", exception.Errors);
    }

    [Fact]
    public async Task OnActionExecutionAsync_ShouldContinue_WhenModelStateIsValid()
    {
        // arrange
        var validatorActionFilter = new ValidatorActionFilter();
        var nextCalled = false;
        
        // act
        await validatorActionFilter.OnActionExecutionAsync(_actionExecutingContext, () =>
        {
            nextCalled = true;
            return Task.FromResult(new ActionExecutedContext(_actionExecutingContext, new List<IFilterMetadata>(), new object()));
        });
        
        // assert
        Assert.True(nextCalled);
    }
}