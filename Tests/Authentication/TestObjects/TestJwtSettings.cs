using FinBookeAPI.Models.Configuration;

namespace FinBookeAPI.Tests.Authentication.TestObjects;

public static class TestJwtSettings
{
    public static JwtSettings GetObject()
    {
        return new JwtSettings
        {
            Audience = "http://dummy-server",
            Issuer = "http://dummy-server",
            Secret = "Por73MjWFc7s5ind78k4AcXEAEtxs0x1k6dZvDHoIUqGzwRDTyUubnGrCeDyZiy1",
        };
    }
}
