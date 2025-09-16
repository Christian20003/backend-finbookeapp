using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FinBookeAPI.Attributes;

/// <summary>
/// This class represents a new data annotation for class properties, fields and parameters that
/// should be a valid color ('#rrggbb'). It should prevent that an unsupported color encoding is provided.
/// </summary>
[AttributeUsage(
    AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    AllowMultiple = false
)]
public class ColorAttribute : ValidationAttribute
{
    /// <summary>
    /// This function proofs if the provided color is a supported encoding.
    /// </summary>
    /// <param name="value">The object that should be analyzed.</param>
    /// <returns>True, if the object is a valid color, otherwise false</returns>
    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return false;
        }
        string pattern = @"^#[0-9a-fA-F]{6}$";
        return Regex.IsMatch(value.ToString() ?? string.Empty, pattern);
    }
}
