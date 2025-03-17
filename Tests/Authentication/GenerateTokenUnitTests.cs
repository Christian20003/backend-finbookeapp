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

public class GenerateTokenUnitTests
{
    // DEPENDENCIES
    private readonly Mock<IAccountManager> UserManager;
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
    private readonly JwtSettings Settings;

    public GenerateTokenUnitTests()
    {
        // Initialize dependencies
        UserManager = MockAccountManager.GetMock();
        SignInManager = MockSignInManager.GetMock(MockUserManager.GetMock());
        AuthDbContext = MockAuthDbContext.GetMock();
        DataProtection = MockDataProtection.GetMock();
        JwtSettings = new Mock<IOptions<JwtSettings>>();
        SmtpServer = new Mock<IOptions<SmtpServer>>();
        Logger = new Mock<ILogger<AuthenticationService>>();

        // Initialize important data objects
        User = TestUserDatabase.GetObject();
        Token = TestRefreshToken.GetObject();
        Request = TestUserTokenRequest.GetObject();
        Settings = TestJwtSettings.GetObject();

        // Mocking methods that have in most cases the same output
        var accounts = new List<UserDatabase> { User }.AsQueryable();
        UserManager
            .Setup(obj => obj.UpdateUserAsync(It.IsAny<UserDatabase>()))
            .ReturnsAsync(IdentityResult.Success);
        UserManager.Setup(obj => obj.GetUsersAsync()).Returns(accounts.ToAsyncEnumerable());
        AuthDbContext
            .Setup(obj => obj.FindRefreshToken(It.IsAny<Expression<Func<RefreshToken, bool>>>()))
            .ReturnsAsync(Token);
        JwtSettings.Setup(obj => obj.Value).Returns(Settings);

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
        var accounts = new List<UserDatabase>().AsQueryable();
        UserManager.Setup(obj => obj.GetUsersAsync()).Returns(accounts.ToAsyncEnumerable());

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.GenerateToken(Request));
    }

    [Fact]
    public async Task Database_Stores_Invalid_UserName()
    {
        User.UserName = null;

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.GenerateToken(Request));
    }

    [Fact]
    public async Task Database_Stores_Invalid_Email()
    {
        User.Email = null;

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.GenerateToken(Request));
    }

    [Fact]
    public async Task RefreshToken_Not_Found()
    {
        AuthDbContext
            .Setup(obj => obj.FindRefreshToken(It.IsAny<Expression<Func<RefreshToken, bool>>>()))
            .ReturnsAsync(() => null);

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.GenerateToken(Request));
    }

    [Fact]
    public async Task Provided_RefreshToken_Is_Invalid()
    {
        var falseToken = new RefreshToken
        {
            Id = Token.Id,
            Token = "123456789",
            UserId = Token.UserId,
            ExpiresAt = Token.ExpiresAt,
        };
        Request.Token = falseToken;

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.GenerateToken(Request));
    }

    [Fact]
    public async Task Provided_RefreshToken_Has_Expired()
    {
        var falseToken = new RefreshToken
        {
            Id = Token.Id,
            Token = Token.Token,
            UserId = Token.UserId,
            ExpiresAt = DateTime.UtcNow.AddDays(-2),
        };
        Request.Token = falseToken;

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.GenerateToken(Request));
    }

    [Fact]
    public async Task Database_Cancels_Operation()
    {
        AuthDbContext
            .Setup(obj => obj.FindRefreshToken(It.IsAny<Expression<Func<RefreshToken, bool>>>()))
            .Throws<OperationCanceledException>();

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.GenerateToken(Request));
    }

    [Fact]
    public async Task JWT_Settings_Not_Set()
    {
        Settings.Secret = null;

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.GenerateToken(Request));
    }

    [Fact]
    public async Task New_Valid_JWT_Generated()
    {
        var result = await Service.GenerateToken(Request);

        Assert.InRange(result.Session.Token.Value.Length, 50, 300);
        Assert.True(DateTime.UtcNow.Ticks < result.Session.Token.Expires);
    }
}
