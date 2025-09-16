using System.ComponentModel.DataAnnotations;

namespace FinBookeAPI.Attributes;

/// <summary>
/// This class represents a new data annotation for class properties, fields and parameters that
/// should be valid GUIDs. It should prevent that an invalid GUID is provided. It does not proof
/// if the GUID is empty.
/// </summary>
[AttributeUsage(
    AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    AllowMultiple = false
)]
public class GuidAttribute : ValidationAttribute
{
    /// <summary>
    /// This function proofs if the provided GUID is a valid GUID object.
    /// </summary>
    /// <param name="value">The object that should be analyzed.</param>
    /// <returns>True, if the object is a valid GUID, otherwise false</returns>
    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return false;
        }
        return Guid.TryParse(value.ToString(), out _);
    }
}
