using Microsoft.Extensions.Compliance.Redaction;

namespace FinBookeAPI.AppConfig.Redaction;

public sealed class StarRedactor : Redactor
{
    private const string _stars = "****";

    public override int GetRedactedLength(ReadOnlySpan<char> input) => _stars.Length;

    public override int Redact(ReadOnlySpan<char> source, Span<char> destination)
    {
        _stars.CopyTo(destination);

        return _stars.Length;
    }
}
