namespace FinBookeAPI.UnitTests.Authentication.Login;

using System.Linq.Expressions;
using FakeItEasy;
using FinBookeAPI.AppConfig;
using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;
using FinBookeAPI.Services;
using FinBookeAPI.UnitTests.Mocks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

public class LoginUnitTests
{
    private readonly UserManager<UserDatabase> UserManager = A.Fake<UserManager<UserDatabase>>();
    private readonly SignInManager<UserDatabase> SignInManager = A.Fake<
        SignInManager<UserDatabase>
    >();
    private readonly Mock<AuthDbContext> AuthDbContext = new(
        new DbContextOptionsBuilder<AuthDbContext>().Options,
        Mock.Of<IOptions<AuthDatabaseSettings>>()
    );

    //A.Fake<AuthDbContext>(o => o.WithArgumentsForConstructor([new DbContextOptionsBuilder<AuthDbContext>().Options, A.Fake<IOptions<AuthDatabaseSettings>>()]));
    //AuthDbContextMock.GetDbMock();
    private readonly IOptions<JwTSettings> JwtSettings = A.Fake<IOptions<JwTSettings>>();
    private readonly IDataProtectionProvider DataProtectionProvider =
        A.Fake<IDataProtectionProvider>();
    private readonly Mock<IDataProtector> DataProtector = new();
    private readonly ILogger<AuthenticationService> Logger = A.Fake<
        ILogger<AuthenticationService>
    >();
    private readonly AuthenticationService Service;
    private readonly UserLogin Data = new()
    {
        Email = "max.mustermann@gmail.com",
        Password = "1234",
    };
    private readonly UserDatabase User = new()
    {
        Id = "2dcafda5-3d7f-4dcc-a420-2f0bd498ae88",
        UserName = "Mustermann",
        PasswordHash = "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4",
    };

    // BEFORE EACH
    public LoginUnitTests()
    {
        //DataProtector.Setup(x => x.Protect(Data.Email)).Returns(Data.Email);
        A.CallTo(() => DataProtectionProvider.CreateProtector("user-data"))
            .Returns(DataProtector.Object);
        Service = new AuthenticationService(
            UserManager,
            SignInManager,
            AuthDbContext.Object,
            JwtSettings,
            DataProtectionProvider,
            Logger
        );
    }

    [Fact]
    public async Task ReceiveInvalidEmailProperty()
    {
        UserDatabase? returnUser = null;
        A.CallTo(() => UserManager.FindByEmailAsync("")).Returns(Task.FromResult(returnUser));

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.Login(Data));
    }

    [Fact]
    public async Task ReceiveInvalidPasswordProperty()
    {
        A.CallTo(() => UserManager.FindByEmailAsync(""))
            .Returns(Task.FromResult<UserDatabase?>(User));
        A.CallTo(() => SignInManager.CheckPasswordSignInAsync(User, Data.Password, true))
            .Returns(Task.FromResult(SignInResult.Failed));

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.Login(Data));
    }

    [Fact]
    public async Task DatabaseStoreInvalidUserName()
    {
        UserDatabase returnUser = User;
        returnUser.UserName = null;
        A.CallTo(() => UserManager.FindByEmailAsync(""))
            .Returns(Task.FromResult<UserDatabase?>(returnUser));
        A.CallTo(() => SignInManager.CheckPasswordSignInAsync(User, Data.Password, true))
            .Returns(Task.FromResult(SignInResult.Success));

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.Login(Data));
    }

    [Fact]
    public async Task DatabaseStoreInvalidEmail()
    {
        UserDatabase returnUser = User;
        returnUser.Email = null;
        A.CallTo(() => UserManager.FindByEmailAsync(""))
            .Returns(Task.FromResult<UserDatabase?>(returnUser));
        A.CallTo(() => SignInManager.CheckPasswordSignInAsync(User, Data.Password, true))
            .Returns(Task.FromResult(SignInResult.Success));

        await Assert.ThrowsAsync<AuthenticationException>(() => Service.Login(Data));
    }

    /* [Fact]
    public async Task CreateAndAddNewRefreshToken()
    {
        RefreshToken? token = null;
        A.CallTo(() => UserManager.FindByEmailAsync("")).Returns(Task.FromResult<UserDatabase?>(User));
        A.CallTo(() => SignInManager.CheckPasswordSignInAsync(User, Data.Password, true)).Returns(Task.FromResult(SignInResult.Success));
        //A.CallTo(() => AuthDbContext.RefreshToken.FirstOrDefaultAsync(A<Expression<Func<RefreshToken, bool>>>.Ignored, A<CancellationToken>.Ignored)).Returns(Task.FromResult(token));
        AuthDbContext.Setup(x => x.RefreshToken.FirstOrDefaultAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(token));

        A.CallTo(() => UserManager.UpdateAsync(A<UserDatabase>.That.IsInstanceOf(typeof(UserDatabase)))).MustHaveHappened();
        //A.CallTo(() => AuthDbContext.RefreshToken.AddAsync(A<RefreshToken>.That.IsInstanceOf(typeof(RefreshToken)), A<CancellationToken>.Ignored)).MustHaveHappened();
    } */
}
