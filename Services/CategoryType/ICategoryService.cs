using FinBookeAPI.Models.CategoryType;
using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Services.CategoryType;

public interface ICategoryService
{
    /// <summary>
    /// This method creates a main category in the database.
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
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// If the userId of a child category is different.
    /// </exception>
    public Task<Category> CreateMainCategory(Category category);

    /// <summary>
    /// This method creates a sub category in the database.
    /// </summary>
    /// <param name="parent">
    /// The id of the parent category.
    /// </param>
    /// <param name="child">
    /// The new sub category.
    /// </param>
    /// <returns>
    /// The sub category that has been added to the database.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the provided sub category name or color is null or empty.
    /// If the provided sub category userId is an empty Guid.
    /// If the provided sub category color is not a valid color encoding.
    /// If the provided sub category children do not exist in the database.
    /// If the provided parent category does not exist.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// If the userId of a child category (from the new sub category) is different.
    /// If the parent and child category have different userIds.
    /// </exception>
    public Task<Category> CreateSubCategory(Guid parent, Category child);

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
