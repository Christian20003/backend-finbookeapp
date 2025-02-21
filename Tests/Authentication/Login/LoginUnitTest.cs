namespace FinBookeAPI.Tests.Authentication.Login;

using System.Linq.Expressions;
using FinBookeAPI.AppConfig;
using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;
using FinBookeAPI.Models.Wrapper;
using FinBookeAPI.Services.Authentication;
using FinBookeAPI.Tests.Authentication.Mocks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

public class LoginUnitTests
{
    // DEPENDENCIES
    private readonly Mock<UserManager<IUserDatabase>> UserManager;
    private readonly Mock<SignInManager<IUserDatabase>> SignInManager;
    private readonly Mock<AuthDbContext> AuthDbContext;
    private readonly Mock<IOptions<IJwtSettings>> JwtSettings;
    private readonly Mock<IDataProtection> DataProtection;
    private readonly Mock<ILogger<AuthenticationService>> Logger;
    private readonly AuthenticationService Service;

    // DATA
    private readonly Mock<IUserLogin> Data;
    private readonly Mock<IUserDatabase> User;
    private readonly Mock<IRefreshToken> Token;
    private readonly Mock<IJwtSettings> Settings;

    // TESTS

    // BEFORE EACH
    public LoginUnitTests()
    {
        // Initialize dependencies
        UserManager = MockUserManager.GetMock();
        SignInManager = MockSignInManager.GetMock(UserManager);
        AuthDbContext = MockAuthDbContext.GetMock();
        DataProtection = MockDataProtection.GetMock();
        JwtSettings = new Mock<IOptions<IJwtSettings>>();
        Logger = new Mock<ILogger<AuthenticationService>>();

        // Initialize important data objects
        User = MockUserDatabase.GetMock();
        Token = MockRefreshToken.GetMock();
        Settings = MockJwtSettings.GetMock();
        Data = MockUserLogin.GetMock();

        // Mocking irrelevant methods
        UserManager
            .Setup(obj => obj.UpdateAsync(It.IsAny<IUserDatabase>()))
            .ReturnsAsync(IdentityResult.Success);
        AuthDbContext
            .Setup(obj => obj.AddRefreshToken(It.IsAny<IRefreshToken>()))
            .ReturnsAsync(Token.Object);
        JwtSettings.Setup(obj => obj.Value).Returns(Settings.Object);

        // Initialize test object
        Service = new AuthenticationService(
            UserManager.Object,
            SignInManager.Object,
            AuthDbContext.Object,
            JwtSettings.Object,
            DataProtection.Object,
            Logger.Object
        );
    }

    [Fact]
    public async Task ReceiveInvalidEmailProperty()
    {
        UserManager.Setup(obj => obj.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(() => null);

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.Login(Data.Object));
    }

    [Fact]
    public async Task ReceiveInvalidPasswordProperty()
    {
        UserManager
            .Setup(obj => obj.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(User.Object);
        SignInManager
            .Setup(obj =>
                obj.CheckPasswordSignInAsync(
                    It.IsAny<IUserDatabase>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()
                )
            )
            .ReturnsAsync(SignInResult.Failed);

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.Login(Data.Object));
    }

    [Fact]
    public async Task DatabaseStoreInvalidUserName()
    {
        User.SetupProperty(obj => obj.UserName, null);
        UserManager
            .Setup(obj => obj.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(User.Object);
        SignInManager
            .Setup(obj =>
                obj.CheckPasswordSignInAsync(
                    It.IsAny<UserDatabase>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()
                )
            )
            .ReturnsAsync(SignInResult.Success);

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.Login(Data.Object));
    }

    [Fact]
    public async Task DatabaseStoreInvalidEmail()
    {
        User.SetupProperty(obj => obj.Email, null);
        UserManager
            .Setup(obj => obj.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(User.Object);
        SignInManager
            .Setup(obj =>
                obj.CheckPasswordSignInAsync(
                    It.IsAny<UserDatabase>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()
                )
            )
            .ReturnsAsync(SignInResult.Success);

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.Login(Data.Object));
    }

    [Fact]
    public async Task CreateAndAddNewRefreshToken()
    {
        UserManager
            .Setup(obj => obj.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(User.Object);
        SignInManager
            .Setup(obj =>
                obj.CheckPasswordSignInAsync(
                    It.IsAny<UserDatabase>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()
                )
            )
            .ReturnsAsync(SignInResult.Success);
        AuthDbContext
            .Setup(obj => obj.FindRefreshToken(It.IsAny<Expression<Func<IRefreshToken, bool>>>()))
            .ReturnsAsync(() => null);

        await Service.Login(Data.Object);

        UserManager.Verify(obj => obj.UpdateAsync(It.IsAny<IUserDatabase>()), Times.Once());
        AuthDbContext.Verify(obj => obj.AddRefreshToken(It.IsAny<IRefreshToken>()), Times.Once());
    }
}
