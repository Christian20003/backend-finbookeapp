using FinBookeAPI.AppConfig.Redaction;
using Microsoft.Extensions.Compliance.Classification;

namespace FinBookeAPI.Attributes;

public sealed class PrivateAttribute : DataClassificationAttribute
{
    public PrivateAttribute()
        : base(RedactionClassification.Private) { }
}
