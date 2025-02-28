using FinBookeAPI.Models.Authentication;

namespace FinBookeAPI.Tests.Authentication.TestObjects;

public static class TestUserDatabase
{
    public static UserDatabase GetObject()
    {
        return new UserDatabase
        {
            Id = "2dcafda5-3d7f-4dcc-a420-2f0bd498ae88",
            UserName = "Mustermann",
            Email = "max.mustermann@gmail.com",
            PasswordHash = "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4",
            RefreshTokenId = "cb8a42ef-739d-4ea3-b0c4-180f33a03955",
        };
    }
}
