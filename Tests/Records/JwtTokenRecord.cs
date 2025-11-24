using FinBookeAPI.Models.Token;

namespace FinBookeAPI.Tests.Records;

public static class JwtTokenRecord
{
    public static JwtToken GetObject()
    {
        return new JwtToken
        {
            Value =
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.KMUFsIDTnFmyG3nMiGM6H9FNFUROf3wh7SmqJp-QV30",
            Expires = DateTime.UtcNow.AddMinutes(2).Ticks,
        };
    }
}
