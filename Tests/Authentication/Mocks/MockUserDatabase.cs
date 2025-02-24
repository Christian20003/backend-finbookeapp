using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Authentication.Interfaces;
using Moq;

namespace FinBookeAPI.Tests.Authentication.Mocks;

public static class MockUserDatabase
{
    public static UserDatabase GetMock()
    {
        return new UserDatabase
        {
            Id = TestData.UserId,
            UserName = TestData.Username,
            Email = TestData.Email,
            PasswordHash = TestData.PasswordHash,
            RefreshTokenId = TestData.TokenId,
        };
    }
}
