using Microsoft.AspNetCore.DataProtection;

namespace FinBookeAPI.Models.Wrapper;

public class DataProtection(IDataProtectionProvider provider) : IDataProtection
{
    public IDataProtector Protector { get; set; } = provider.CreateProtector("protection");

    public string Protect(string value)
    {
        return Protector.Protect(value);
    }

    public string ProtectEmail(string value)
    {
        var index = value.IndexOf('@');
        if (index == -1)
        {
            throw new ArgumentException("Provided value is not an email");
        }
        Console.WriteLine(Protector.Protect(value[..index]));
        return Protector.Protect(value[..index]) + value[index..];
    }

    public string Unprotect(string value)
    {
        return Protector.Unprotect(value);
    }

    public string UnprotectEmail(string value)
    {
        var index = value.IndexOf('@');
        if (index == -1)
        {
            throw new ArgumentException("Provided value is not an email");
        }
        return Protector.Unprotect(value[..index]) + value[index..];
    }
}
