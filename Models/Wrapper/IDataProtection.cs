using Microsoft.AspNetCore.DataProtection;

namespace FinBookeAPI.Models.Wrapper;

public interface IDataProtection
{
    public IDataProtector Protector { get; set; }

    /// <summary>
    /// This method protects the provided <c>value</c> with cryptographical methods.
    /// </summary>
    /// <param name="value">
    /// The values which should be protected.
    /// </param>
    /// <returns>
    /// The protected value.
    /// </returns>
    public string Protect(string value);

    /// <summary>
    /// This method unprotects the provided <c>value</c> to get the underlying plaintext.
    /// </summary>
    /// <param name="value">
    /// The value which should be unprotected.
    /// </param>
    /// <returns>
    /// The plaintext of the provided protected <c>value</c>.
    /// </returns>
    public string Unprotect(string value);

    /// <summary>
    /// This method protects email address with cryptographical methods. Thereby every
    /// character before the <c>@</c> symbol will be protected, so that specific requirement
    /// annotations are not violated. If the provided string does not contain any <c>@</c> symbol
    /// an <c>ArgumentException</c> will be thrown.
    /// </summary>
    /// <param name="value">
    /// The email which should be protected.
    /// </param>
    /// <returns>
    /// The protected email.
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    public string ProtectEmail(string value);

    /// <summary>
    /// This method unprotects the provided email address. If the provided string does not contain
    /// any <c>@</c> symbol an <c>ArgumentException</c> will be thrown.
    /// </summary>
    /// <param name="value">
    /// The email which should be unprotected.
    /// </param>
    /// <returns>
    /// The plaintext email address.
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    public string UnprotectEmail(string value);
}
