using Microsoft.AspNetCore.DataProtection;

namespace FinBookeAPI.Models.Wrapper;

public class DataProtection(IDataProtectionProvider provider) : IDataProtection
{
    public IDataProtector Protector { get; set; } = provider.CreateProtector("protection");

    public string Protect(string value)
    {
        return Protector.Protect(value);
    }

    public string Unprotect(string value)
    {
        return Protector.Unprotect(value);
    }
}
