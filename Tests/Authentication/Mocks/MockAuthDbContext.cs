using FinBookeAPI.AppConfig;
using FinBookeAPI.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

namespace FinBookeAPI.Tests.Authentication.Mocks;

public static class MockAuthDbContext
{
    public static Mock<AuthDbContext> GetMock()
    {
        var obj = new Mock<AuthDbContext>(
            new DbContextOptionsBuilder<AuthDbContext>().Options,
            new Mock<IOptions<AuthDatabaseSettings>>().Object
        );
        return obj;
    }
}
