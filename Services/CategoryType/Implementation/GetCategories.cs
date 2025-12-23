using FinBookeAPI.AppConfig.Documentation;
using FinBookeAPI.Models.CategoryType;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Services.CategoryType;

public partial class CategoryService : ICategoryService
{
    public async Task<IEnumerable<Category>> GetCategories(Guid userId)
    {
        _logger.LogDebug("Read all categories of {id}", userId);

        if (userId == Guid.Empty)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryReadFailed,
                new ArgumentException("UserId is an empty Guid", nameof(userId))
            );

        var categories = await _collection.GetCategories(category => category.UserId == userId);
        var result = categories.Select(category => new Category(category));

        _logger.LogInformation(
            LogEvents.CategoryReadSuccess,
            "Successfully read all categories of {user}",
            userId
        );

        return result;
    }

    public async Task<Category> GetCategory(Guid categoryId, Guid userId)
    {
        _logger.LogDebug("Read category {category}", categoryId);

        var entity = await VerifyCategoryAccess(categoryId, userId);

        _logger.LogInformation(
            LogEvents.CategoryReadSuccess,
            "Successfully read {category}",
            entity.ToString()
        );

        return new Category(entity);
    }
}
