using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using To_Do_List.Application.Common.Exceptions;
using To_Do_List.Application.Common.Filters;
using To_Do_List.Domain.Models;

namespace To_Do_List.Application.UnitTests.Filters;

public class AuthorizationFilterTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly ActionExecutingContext _actionExecutingContext;

    public AuthorizationFilterTests()
    {
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

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
    public async Task OnActionExecutionAsync_ShouldContinue_WhenUserIsFound()
    {
        // arrange
        _mockUserManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(new ApplicationUser());

        var authorizationFilter = new AuthorizationFilter(_mockUserManager.Object);

        // act
        var nextCalled = false;
        await authorizationFilter.OnActionExecutionAsync(_actionExecutingContext, () =>
        {
            nextCalled = true;
            return Task.FromResult(new ActionExecutedContext(_actionExecutingContext, new List<IFilterMetadata>(), new object()));
        });

        // assert
        Assert.True(nextCalled);
    }

    [Fact]
    public async Task OnActionExecutionAsync_ShouldThrowUnauthorizedException_WhenUserIsNotFound()
    {
        // arrange
        _mockUserManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync((ApplicationUser?)null);

        var authorizationFilter = new AuthorizationFilter(_mockUserManager.Object);
        
        // assert & ast
        await Assert.ThrowsAsync<UnauthorizedException>(() => authorizationFilter.OnActionExecutionAsync(_actionExecutingContext, () => 
            Task.FromResult(new ActionExecutedContext(_actionExecutingContext, new List<IFilterMetadata>(), new object()))));
    }
}