using FinBookeAPI.AppConfig;
using FinBookeAPI.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FinBookeAPI.UnitTests.Mocks;

public static class AuthDbContextMock
{
    public static AuthDbContext GetDbMock()
    {
        var options = new DbContextOptionsBuilder<AuthDbContext>().Options;
        var config = new AuthDatabaseSettings
        {
            ConnectionString = "mongodb://localhost:8888",
            DatabaseName = "Test",
        };
        var settings = Options.Create(config);
        var context = new AuthDbContext(options, settings);
        //A.CallTo(() => context.RefreshToken).Returns(A.Fake<DbSet<RefreshToken>>());
        return context;
    }
}
