using FinBookeAPI.Models.Authentication;

namespace FinBookeAPI.Tests.Authentication.TestObjects;

public static class TestUserLogin
{
    public static UserLogin GetObject()
    {
        return new UserLogin { Email = "max.mustermann@gmail.com", Password = "1234" };
    }
}
