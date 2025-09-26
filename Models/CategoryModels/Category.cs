using System.ComponentModel.DataAnnotations;
using FinBookeAPI.Attributes;

namespace FinBookeAPI.Models.CategoryModels;

public class Category
{
    /// <summary>
    /// The id of the category. If not set a new id will be generated.
    /// </summary>
    [Guid(ErrorMessage = "Provided GUID of category is not valid")]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// The id of the user who created the category. If not set, the category will be available for all users.
    /// </summary>
    [Guid(ErrorMessage = "Provided GUID of the user is not valid")]
    public Guid UserId { get; set; } = Guid.Empty;

    /// <summary>
    /// The name of the category.
    /// </summary>
    [Required(ErrorMessage = "Provided category name is missing")]
    public string Name { get; set; } = "";

    /// <summary>
    /// The ids of all subcategories
    /// </summary>
    [Guid(ErrorMessage = "Provided GUIDs of subcategories are not valid")]
    public IEnumerable<Guid> Children { get; set; } = [];

    /// <summary>
    /// The color of the category (in hex, rgb, cmyk or hsl encoding)
    /// </summary>
    [Color(ErrorMessage = "Provided category color is an unsupported encoding")]
    public string Color { get; set; } = "";

    /// <summary>
    /// The date where this category was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The date where this category was modified
    /// </summary>
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
}
