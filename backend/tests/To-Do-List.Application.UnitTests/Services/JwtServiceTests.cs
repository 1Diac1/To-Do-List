using To_Do_List.Application.Common.Helpers;
using System.IdentityModel.Tokens.Jwt;
using To_Do_List.Application.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using To_Do_List.Domain.Models;
using System.Text;
using Moq;

namespace To_Do_List.Application.UnitTests.Services;

public class JwtServiceTests
{
    private readonly Mock<IOptions<JwtOptions>> _mockOptions;

    public JwtServiceTests()
    {
        _mockOptions = new Mock<IOptions<JwtOptions>>();
    }

    [Fact]
    public void GenerateToken_ShouldGenerateValidToken()
    {
        // arrange
        var applicationUser = new ApplicationUser()
        {
            UserName = "Something username",
            Email = "Something email"
        };

        var jwtOptions = new JwtOptions("issuer", "audience", "SomethingSecurityKey", 10);

        _mockOptions.Setup(o => o.Value).Returns(jwtOptions);
        
        var jwtService = new JwtService(_mockOptions.Object);
        
        // act
        var token = jwtService.GenerateAccessToken(applicationUser);
        
        var validations = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
    
            ValidIssuer = _mockOptions.Object.Value.Issuer,
            ValidAudience = _mockOptions.Object.Value.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_mockOptions.Object.Value.Key))
        };
        
        var jwtToken = new JwtSecurityTokenHandler().ValidateToken(token, validations, out var tokenSecure);
        
        // assert
        Assert.Equal(applicationUser.UserName, jwtToken?.Identity?.Name);
    }

    [Fact]
    public void GenerateToken_ShouldThrowArgumentException_WhenCreateExpiredToken()
    {
        // arrange
        var jwtOptions = new JwtOptions("issuer", "audience", "SomethingSecurityKey", -30);
        var applicationUser = new ApplicationUser() { UserName = "Something username", Email = "Something email" };

        _mockOptions.Setup(o => o.Value).Returns(jwtOptions);
        
        var jwtService = new JwtService(_mockOptions.Object);

        // assert & act
        Assert.Throws<ArgumentException>(() => jwtService.GenerateAccessToken(applicationUser));
    }

    [Theory] 
    [InlineData("")]
    [InlineData("123456")]
    [InlineData("1234567890abcdef")]
    public void GenerateToken_ShouldThrowExceptions_WhenInsertKeyWithInvalidLength(string key)
    {
        // arrange
        var jwtOptions = new JwtOptions("issuer", "audience", key, 30);
        var applicationUser = new ApplicationUser() { UserName = "Something username", Email = "Something email" };

        _mockOptions.Setup(o => o.Value).Returns(jwtOptions);

        var jwtService = new JwtService(_mockOptions.Object);
        
        // assert
        if (key.Length < 16)
            Assert.ThrowsAny<Exception>(() => jwtService.GenerateAccessToken(applicationUser));
        else
            Assert.NotNull(jwtService.GenerateAccessToken(applicationUser));
    }
}