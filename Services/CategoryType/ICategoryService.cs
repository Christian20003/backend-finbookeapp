using FinBookeAPI.Models.CategoryType;
using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Services.CategoryType;

public interface ICategoryService
{
    /// <summary>
    /// This method creates a category in the database.
    /// </summary>
    /// <param name="category">
    /// The category that should be added.
    /// </param>
    /// <returns>
    /// The category that has been added to the database.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the provided category name or color is null or empty.
    /// If the provided category userId is an empty Guid.
    /// If the provided category color is not a valid color encoding.
    /// If the provided category children do not exist in the database.
    /// If the provided category parent does not exist in the database.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// If the userId of a child category is different.
    /// If the userId of the parent category is different.
    /// </exception>
    public Task<Category> CreateCategory(Category category);

    /// <summary>
    /// This method returns all categories that corresponds to the user.
    /// </summary>
    /// <param name="userId">
    /// The user id.
    /// </param>
    /// <returns>
    /// All categories in a nested structure.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the user id is an empty Guid.
    /// </exception>
    public Task<IEnumerable<CategoryNested>> GetCategories(Guid userId);
}
