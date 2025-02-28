using FinBookeAPI.Models.Authentication;

namespace FinBookeAPI.Tests.Authentication.TestObjects;

public static class TestUserTokenRequest
{
    public static UserTokenRequest GetObject()
    {
        return new UserTokenRequest
        {
            Email = "max.mustermann@gmail.com",
            Token = new RefreshToken
            {
                Id = "cb8a42ef-739d-4ea3-b0c4-180f33a03955",
                Token = "7dlLr68EpJllZlSSqzSN1lc2WjosQYorgHtwhnjXlNM3LOutam38YLb2WvOMMkJW",
                ExpiresAt = DateTime.UtcNow.AddDays(1),
            },
        };
    }
}
