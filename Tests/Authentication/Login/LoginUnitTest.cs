namespace FinBookeAPI.Tests.Authentication.Login;

using System.Linq.Expressions;
using FinBookeAPI.AppConfig;
using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;
using FinBookeAPI.Models.Wrapper;
using FinBookeAPI.Services.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

public class LoginUnitTests
{
    // DEPENDENCIES
    private readonly Mock<UserManager<UserDatabase>> UserManager;
    private readonly Mock<SignInManager<UserDatabase>> SignInManager;
    private readonly Mock<AuthDbContext> AuthDbContext;
    private readonly Mock<IOptions<JwTSettings>> JwtSettings;
    private readonly Mock<IDataProtection> DataProtection;
    private readonly Mock<ILogger<AuthenticationService>> Logger;
    private readonly AuthenticationService Service;

    // DUMMY DATA
    private readonly UserLogin Data = new()
    {
        Email = "max.mustermann@gmail.com",
        Password = "1234",
    };
    private readonly UserDatabase User = new()
    {
        Id = "2dcafda5-3d7f-4dcc-a420-2f0bd498ae88",
        UserName = "Mustermann",
        Email = "max.mustermann@gmail.com",
        PasswordHash = "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4",
    };
    private readonly RefreshToken Token = new()
    {
        Id = "b8d48c7c-791e-4bed-9c63-f24e2b79341b",
        Token = "7dlLr68EpJllZlSSqzSN1lc2WjosQYorgHtwhnjXlNM3LOutam38YLb2WvOMMkJW",
        UserId = "2dcafda5-3d7f-4dcc-a420-2f0bd498ae88",
        ExpiresAt = DateTime.UtcNow.AddHours(5),
    };
    private readonly JwTSettings Settings = new()
    {
        Secret = "Por73MjWFc7s5ind78k4AcXEAEtxs0x1k6dZvDHoIUqGzwRDTyUubnGrCeDyZiy1",
    };

    // TESTS

    // BEFORE EACH
    public LoginUnitTests()
    {
        // Initialize dependencies
        UserManager = new Mock<UserManager<UserDatabase>>(
            Mock.Of<IUserStore<UserDatabase>>(),
            Mock.Of<IOptions<IdentityOptions>>(),
            Mock.Of<IPasswordHasher<UserDatabase>>(),
            FakeItEasy.A.Fake<IEnumerable<IUserValidator<UserDatabase>>>(),
            FakeItEasy.A.Fake<IEnumerable<IPasswordValidator<UserDatabase>>>(),
            Mock.Of<ILookupNormalizer>(),
            Mock.Of<IdentityErrorDescriber>(),
            Mock.Of<IServiceProvider>(),
            Mock.Of<ILogger<UserManager<UserDatabase>>>()
        );

        SignInManager = new Mock<SignInManager<UserDatabase>>(
            UserManager.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<UserDatabase>>(),
            Mock.Of<IOptions<IdentityOptions>>(),
            Mock.Of<ILogger<SignInManager<UserDatabase>>>(),
            Mock.Of<Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider>(),
            Mock.Of<IUserConfirmation<UserDatabase>>()
        );

        AuthDbContext = new Mock<AuthDbContext>(
            new DbContextOptionsBuilder<AuthDbContext>().Options,
            new Mock<IOptions<AuthDatabaseSettings>>().Object
        );

        JwtSettings = new Mock<IOptions<JwTSettings>>();
        DataProtection = new Mock<IDataProtection>();
        Logger = new Mock<ILogger<AuthenticationService>>();

        // DataProtection object should do nothing
        DataProtection.Setup(obj => obj.Protect(Data.Email)).Returns(Data.Email);
        DataProtection.Setup(obj => obj.Unprotect(User.Email)).Returns(User.Email);
        DataProtection.Setup(obj => obj.Unprotect(User.UserName)).Returns(User.UserName);

        // Mocking database activities
        UserManager.Setup(obj => obj.UpdateAsync(User)).ReturnsAsync(IdentityResult.Success);
        AuthDbContext
            .Setup(obj => obj.AddRefreshToken(It.IsAny<RefreshToken>()))
            .ReturnsAsync(Token);

        JwtSettings.Setup(obj => obj.Value).Returns(Settings);

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
        UserDatabase? returnUser = null;
        UserManager.Setup(obj => obj.FindByEmailAsync(Data.Email)).ReturnsAsync(returnUser);

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.Login(Data));
    }

    [Fact]
    public async Task ReceiveInvalidPasswordProperty()
    {
        UserManager.Setup(obj => obj.FindByEmailAsync(Data.Email)).ReturnsAsync(User);
        SignInManager
            .Setup(obj =>
                obj.CheckPasswordSignInAsync(
                    It.IsAny<UserDatabase>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()
                )
            )
            .ReturnsAsync(SignInResult.Failed);

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.Login(Data));
    }

    [Fact]
    public async Task DatabaseStoreInvalidUserName()
    {
        UserDatabase returnUser = User;
        returnUser.UserName = null;
        UserManager.Setup(obj => obj.FindByEmailAsync(Data.Email)).ReturnsAsync(User);
        SignInManager
            .Setup(obj =>
                obj.CheckPasswordSignInAsync(
                    It.IsAny<UserDatabase>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()
                )
            )
            .ReturnsAsync(SignInResult.Success);

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.Login(Data));
    }

    [Fact]
    public async Task DatabaseStoreInvalidEmail()
    {
        UserDatabase returnUser = User;
        returnUser.Email = null;
        UserManager.Setup(obj => obj.FindByEmailAsync(Data.Email)).ReturnsAsync(User);
        SignInManager
            .Setup(obj => obj.CheckPasswordSignInAsync(User, Data.Password, true))
            .ReturnsAsync(SignInResult.Success);

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.Login(Data));
    }

    [Fact]
    public async Task CreateAndAddNewRefreshToken()
    {
        RefreshToken? token = null;
        UserManager.Setup(obj => obj.FindByEmailAsync(Data.Email)).ReturnsAsync(User);
        SignInManager
            .Setup(obj => obj.CheckPasswordSignInAsync(User, Data.Password, true))
            .ReturnsAsync(SignInResult.Success);
        AuthDbContext
            .Setup(x => x.FindRefreshToken(It.IsAny<Expression<Func<RefreshToken, bool>>>()))
            .Returns(Task.FromResult(token));

        await Service.Login(Data);

        UserManager.Verify(obj => obj.UpdateAsync(User), Times.Once());
        AuthDbContext.Verify(obj => obj.AddRefreshToken(It.IsAny<RefreshToken>()), Times.Once());
    }
}
