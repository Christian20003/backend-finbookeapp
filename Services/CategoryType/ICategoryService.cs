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
    /// If the category name or color is null or empty.
    /// If the category userId is an empty Guid.
    /// If the category color is not a valid color encoding.
    /// If the category limit amount is smaller or equal to zero.
    /// If the category limit amount is larger than that of its potential parent.
    /// If the category limit amount is smaller than the sum of its potential children.
    /// If the category limit period is smaller or equal to zero.
    /// If the category children do not exist in the database.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// If the category color is not a valid color format.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// If the userId of a child category is different.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If tracking operations have been canceled.
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
