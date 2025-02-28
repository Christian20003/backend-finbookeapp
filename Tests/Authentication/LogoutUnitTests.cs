using System.Linq.Expressions;
using FinBookeAPI.AppConfig;
using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;
using FinBookeAPI.Models.Wrapper;
using FinBookeAPI.Services.Authentication;
using FinBookeAPI.Tests.Authentication.Mocks;
using FinBookeAPI.Tests.Authentication.TestObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;

namespace FinBookeAPI.Tests.Authentication;

public class LogoutUnitTests
{
    // DEPENDENCIES
    private readonly Mock<UserManager<UserDatabase>> UserManager;
    private readonly Mock<SignInManager<UserDatabase>> SignInManager;
    private readonly Mock<AuthDbContext> AuthDbContext;
    private readonly Mock<IOptions<JwtSettings>> JwtSettings;
    private readonly Mock<IOptions<SmtpServer>> SmtpServer;
    private readonly Mock<IDataProtection> DataProtection;
    private readonly Mock<ILogger<AuthenticationService>> Logger;
    private readonly AuthenticationService Service;

    // DATA
    private readonly UserDatabase User;
    private readonly UserTokenRequest Request;
    private readonly RefreshToken Token;

    public LogoutUnitTests()
    {
        // Initialize dependencies
        UserManager = MockUserManager.GetMock();
        SignInManager = MockSignInManager.GetMock(UserManager);
        AuthDbContext = MockAuthDbContext.GetMock();
        DataProtection = MockDataProtection.GetMock();
        JwtSettings = new Mock<IOptions<JwtSettings>>();
        SmtpServer = new Mock<IOptions<SmtpServer>>();
        Logger = new Mock<ILogger<AuthenticationService>>();

        // Initialize important data objects
        User = TestUserDatabase.GetObject();
        Token = TestRefreshToken.GetObject();
        Request = TestUserTokenRequest.GetObject();

        // Mocking methods that have in most cases the same output
        UserManager
            .Setup(obj => obj.UpdateAsync(It.IsAny<UserDatabase>()))
            .ReturnsAsync(IdentityResult.Success);
        UserManager.Setup(obj => obj.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(User);
        AuthDbContext
            .Setup(obj => obj.FindRefreshToken(It.IsAny<Expression<Func<RefreshToken, bool>>>()))
            .ReturnsAsync(Token);
        AuthDbContext.Setup(obj => obj.RemoveRefreshToken(It.IsAny<string>())).ReturnsAsync(Token);

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
    public async Task User_Account_Not_Found()
    {
        UserManager.Setup(obj => obj.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(() => null);

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.Logout(Request));
    }

    [Fact]
    public async Task User_Has_Invalid_Username()
    {
        User.UserName = null;

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.Logout(Request));
    }

    [Fact]
    public async Task User_Has_Invalid_Email()
    {
        User.Email = null;

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.Logout(Request));
    }

    [Fact]
    public async Task Refresh_Token_Not_Found_In_CheckRefreshToken()
    {
        AuthDbContext
            .Setup(obj => obj.FindRefreshToken(It.IsAny<Expression<Func<RefreshToken, bool>>>()))
            .ReturnsAsync(() => null);

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.Logout(Request));
    }

    [Fact]
    public async Task Refresh_Token_Is_Invalid()
    {
        Token.Token = "abcde";

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.Logout(Request));
    }

    [Fact]
    public async Task RefreshToken_Has_Expired()
    {
        Token.ExpiresAt = DateTime.UtcNow.AddDays(-2);

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.Logout(Request));
    }

    [Fact]
    public async Task Refresh_Token_Not_Found_In_RemoveRefreshToken()
    {
        AuthDbContext
            .Setup(obj => obj.RemoveRefreshToken(It.IsAny<string>()))
            .Throws<NullReferenceException>();

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.Logout(Request));
    }

    [Fact]
    public async Task Database_Cancels_Operation()
    {
        AuthDbContext
            .Setup(obj => obj.FindRefreshToken(It.IsAny<Expression<Func<RefreshToken, bool>>>()))
            .Throws<OperationCanceledException>();

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.Logout(Request));
    }

    [Fact]
    public async Task Successful_logout()
    {
        await Service.Logout(Request);

        UserManager.Verify(obj => obj.UpdateAsync(It.IsAny<UserDatabase>()), Times.Once());
        Assert.Equal("", User.RefreshTokenId);
    }
}
