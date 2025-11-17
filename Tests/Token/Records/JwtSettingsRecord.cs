using FinBookeAPI.Models.Configuration;

namespace FinBookeAPI.Tests.Token.Records;

public static class JwtSettingsRecord
{
    public static JwtSettings GetObject()
    {
        return new JwtSettings
        {
            Audience = "http://audience",
            Issuer = "http://issuer",
            AccessTokenSecret = "Por73MjWFc7s5ind78k4AcXEAEtxs0x1k6dZvDHoIUqGzwRDTyUubnGrCeDyZiy1",
            RefreshTokenSecret = "Por73MjWFc7s5ind78k4AcXEAEtxs0x1k6dZvDHoIUqGzwRDTyUubnGrCeDyZiy1",
        };
    }
}
