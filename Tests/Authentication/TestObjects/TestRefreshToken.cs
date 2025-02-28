using FinBookeAPI.Models.Authentication;

namespace FinBookeAPI.Tests.Authentication.TestObjects;

public static class TestRefreshToken
{
    public static RefreshToken GetObject()
    {
        return new RefreshToken
        {
            Id = "cb8a42ef-739d-4ea3-b0c4-180f33a03955",
            UserId = "2dcafda5-3d7f-4dcc-a420-2f0bd498ae88",
            Token = "c931fa41647ab54ba67d7b6ec7d265070b423544ecc197f03b266825529b1978",
            ExpiresAt = DateTime.UtcNow.AddDays(1),
        };
    }
}
