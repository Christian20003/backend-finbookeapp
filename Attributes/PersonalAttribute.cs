using FinBookeAPI.AppConfig.Redaction;
using Microsoft.Extensions.Compliance.Classification;

namespace FinBookeAPI.Attributes;

public sealed class PersonalAttribute : DataClassificationAttribute
{
    public PersonalAttribute()
        : base(RedactionClassification.Personal) { }
}
