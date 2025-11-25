using Microsoft.Extensions.Compliance.Redaction;

namespace FinBookeAPI.AppConfig.Redaction;

/// <summary>
/// This static class is only there to provide a function to hide primitive data
/// </summary>
public static class PrivacyGuard
{
    /// <summary>
    /// This method hide sensitive data that should not be inside log messages.
    /// </summary>
    /// <param name="provider">
    /// The provider which returns a redactor component.
    /// </param>
    /// <param name="msg">
    /// The data that should be masked.
    /// </param>
    /// <returns>
    /// The masked data.
    /// </returns>
    public static string Hide(IRedactorProvider provider, string? msg)
    {
        return provider.GetRedactor(RedactionClassification.Private).Redact(msg);
    }
}
