using Microsoft.AspNetCore.DataProtection;

namespace FinBookeAPI.Models.Wrapper;

public interface IDataProtection
{
    public IDataProtector Protector { get; set; }
    public string Protect(string value);
    public string Unprotect(string value);
}
