using FinBookeAPI.Models.CategoryType;

namespace FinBookeAPI.Services.CategoryType;

public partial class CategoryService : ICategoryService
{
    /// <summary>
    /// This method transforms a <see cref="Category"/> into a
    /// <see cref="CategoryNested"/> object recursively.
    /// </summary>
    /// <param name="category">
    /// The category that should be transformed.
    /// </param>
    /// <param name="categories">
    /// A dictionary of categories that could be a child.
    /// </param>
    /// <returns>
    /// A <see cref="CategoryNested"/> object.
    /// </returns>
    private static CategoryNested TransformCategory(
        Category category,
        Dictionary<Guid, Category> categories
    )
    {
        var result = new CategoryNested(category);
        foreach (var childId in category.Children)
        {
            try
            {
                var child = TransformCategory(categories[childId], categories);
                result.Children = result.Children.Append(child);
            }
            catch (KeyNotFoundException)
            {
                continue;
            }
        }
        return result;
    }
}
