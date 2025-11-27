using System.Text;

namespace FinBookeAPI.Models.CategoryType;

public class Category
{
    /// <summary>
    /// The id of the category. If not set a new id will be generated.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// The id of the user who created the category.
    /// </summary>
    public Guid UserId { get; set; } = Guid.Empty;

    /// <summary>
    /// The name of the category.
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// The ids of all subcategories
    /// </summary>
    public IEnumerable<Guid> Children { get; set; } = [];

    /// <summary>
    /// The color of the category (in hex, rgb, cmyk or hsl encoding)
    /// </summary>
    public string Color { get; set; } = "";

    /// <summary>
    /// The date where this category was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The date where this category was modified
    /// </summary>
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

    public Category() { }

    public Category(Category category)
    {
        Id = category.Id;
        Name = category.Name;
        UserId = category.UserId;
        Color = category.Color;
        CreatedAt = category.CreatedAt;
        ModifiedAt = category.ModifiedAt;
    }

    public override string ToString()
    {
        return $"Category: {{ Id: {Id}, Name: {Name}, UserId: {UserId}, Children: [{string.Join(", ", Children)}], Color: {Color}, CreatedAt: {CreatedAt}, ModifiedAt: {ModifiedAt} }}";
    }
}
