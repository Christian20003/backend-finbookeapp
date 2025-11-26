using FinBookeAPI.Models.Configuration;

namespace FinBookeAPI.Services.CategoryType;

public partial class CategoryService : ICategoryService
{
    /// <summary>
    /// This method verifies if the provided children exist in the database
    /// and if the user has access to them.
    /// </summary>
    /// <param name="userId">
    /// The user of the child categories.
    /// </param>
    /// <param name="children">
    /// The child categories that should be verified.
    /// </param>
    /// <exception cref="ArgumentException">
    /// If at least a single child does not exist in the database
    /// or the user does not have access to that category
    /// (does not exist on the user perspective).
    /// </exception>
    private async Task VerifyCategoryChilds(Guid userId, IEnumerable<Guid> children)
    {
        if (children.Any() && await _collection.ExistCategories(children, userId))
        {
            _logger.LogWarning(
                LogEvents.CategoryOperationFailed,
                "At least one of these categories does not exist {ids}",
                children.ToString()
            );
            throw new ArgumentException(
                "One or more of the provided child categories does not exist",
                nameof(children)
            );
        }
    }
}
