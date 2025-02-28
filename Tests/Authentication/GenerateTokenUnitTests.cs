using System.Linq.Expressions;
using FinBookeAPI.AppConfig;
using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Authentication.Interfaces;
using FinBookeAPI.Models.Configuration.Interfaces;
using FinBookeAPI.Models.Exceptions;
using FinBookeAPI.Models.Wrapper;
using FinBookeAPI.Services.Authentication;
using FinBookeAPI.Tests.Authentication.Mocks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;

namespace FinBookeAPI.Tests.Authentication;

public class GenerateTokenUnitTests
{
    // DEPENDENCIES
    private readonly Mock<UserManager<UserDatabase>> UserManager;
    private readonly Mock<SignInManager<UserDatabase>> SignInManager;
    private readonly Mock<AuthDbContext> AuthDbContext;
    private readonly Mock<IOptions<IJwtSettings>> JwtSettings;
    private readonly Mock<IOptions<ISmtpServer>> SmtpServer;
    private readonly Mock<IDataProtection> DataProtection;
    private readonly Mock<ILogger<AuthenticationService>> Logger;
    private readonly AuthenticationService Service;

    // DATA
    private readonly UserDatabase User;
    private readonly Mock<IUserTokenRequest> Request;
    private readonly Mock<IRefreshToken> Token;
    private readonly Mock<IJwtSettings> Settings;

    public GenerateTokenUnitTests()
    {
        // Initialize dependencies
        UserManager = MockUserManager.GetMock();
        SignInManager = MockSignInManager.GetMock(UserManager);
        AuthDbContext = MockAuthDbContext.GetMock();
        DataProtection = MockDataProtection.GetMock();
        JwtSettings = new Mock<IOptions<IJwtSettings>>();
        SmtpServer = new Mock<IOptions<ISmtpServer>>();
        Logger = new Mock<ILogger<AuthenticationService>>();

        // Initialize important data objects
        User = MockUserDatabase.GetMock();
        Token = MockRefreshToken.GetMock();
        Request = MockUserTokenRequest.GetMock();
        Settings = MockJwtSettings.GetMock();

        // Mocking methods that have in most cases the same output
        UserManager
            .Setup(obj => obj.UpdateAsync(It.IsAny<UserDatabase>()))
            .ReturnsAsync(IdentityResult.Success);
        UserManager.Setup(obj => obj.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(User);
        AuthDbContext
            .Setup(obj => obj.FindRefreshToken(It.IsAny<Expression<Func<IRefreshToken, bool>>>()))
            .ReturnsAsync(Token.Object);
        JwtSettings.Setup(obj => obj.Value).Returns(Settings.Object);

        // Initialize test object
        Service = new AuthenticationService(
            UserManager.Object,
            SignInManager.Object,
            AuthDbContext.Object,
            JwtSettings.Object,
            SmtpServer.Object,
            DataProtection.Object,
            Logger.Object
        );
    }

    [Fact]
    public async Task Receive_Invalid_Email_Property()
    {
        UserManager.Setup(obj => obj.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(() => null);

        await Assert.ThrowsAsync<AuthenticationException>(
            () => Service.GenerateToken(Request.Object)
        );
    }

    [Fact]
    public async Task Database_Stores_Invalid_UserName()
    {
        User.UserName = null;

        await Assert.ThrowsAsync<AuthenticationException>(
            () => Service.GenerateToken(Request.Object)
        );
    }

    [Fact]
    public async Task Database_Stores_Invalid_Email()
    {
        User.Email = null;

        await Assert.ThrowsAsync<AuthenticationException>(
            () => Service.GenerateToken(Request.Object)
        );
    }

    [Fact]
    public async Task RefreshToken_Not_Found()
    {
        AuthDbContext
            .Setup(obj => obj.FindRefreshToken(It.IsAny<Expression<Func<IRefreshToken, bool>>>()))
            .ReturnsAsync(() => null);

        await Assert.ThrowsAsync<AuthenticationException>(
            () => Service.GenerateToken(Request.Object)
        );
    }

    [Fact]
    public async Task Provided_RefreshToken_Is_Invalid()
    {
        var falseToken = new Mock<IRefreshToken>();
        falseToken.SetupProperty(obj => obj.Token, "123456789");
        Request.SetupProperty(obj => obj.Token, falseToken.Object);

        await Assert.ThrowsAsync<AuthenticationException>(
            () => Service.GenerateToken(Request.Object)
        );
    }

    [Fact]
    public async Task Provided_RefreshToken_Has_Expired()
    {
        Token.SetupProperty(obj => obj.ExpiresAt, DateTime.UtcNow.AddDays(-2));

        await Assert.ThrowsAsync<AuthenticationException>(
            () => Service.GenerateToken(Request.Object)
        );
    }

    [Fact]
    public async Task Database_Cancels_Operation()
    {
        AuthDbContext
            .Setup(obj => obj.FindRefreshToken(It.IsAny<Expression<Func<IRefreshToken, bool>>>()))
            .Throws<OperationCanceledException>();

        await Assert.ThrowsAsync<AuthenticationException>(
            () => Service.GenerateToken(Request.Object)
        );
    }

    [Fact]
    public async Task JWT_Settings_Not_Set()
    {
        Settings.SetupProperty(obj => obj.Secret, null);

        await Assert.ThrowsAsync<AuthenticationException>(
            () => Service.GenerateToken(Request.Object)
        );
    }

    [Fact]
    public async Task New_Valid_JWT_Generated()
    {
        var result = await Service.GenerateToken(Request.Object);

        Assert.InRange(result.Session.Token.Value.Length, 50, 200);
        Assert.True(DateTime.UtcNow.Ticks < result.Session.Token.Expires);
    }
}
