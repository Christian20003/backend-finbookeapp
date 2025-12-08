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

        var categories = await _collection.GetCategories(userId);
        var result = new List<Category>();
        foreach (var category in categories)
            result.Add(new Category(category));

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

        if (categoryId == Guid.Empty)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryReadFailed,
                new ArgumentException("CategoryId is an empty Guid", nameof(categoryId))
            );
        if (userId == Guid.Empty)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryReadFailed,
                new ArgumentException("UserId is an empty Guid", nameof(userId))
            );

        var category = await _collection.GetCategory(categoryId);
        if (category is null)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryReadFailed,
                new EntityNotFoundException("Category does not exist")
            );
        if (category.UserId != userId)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryReadFailed,
                new AuthorizationException("Category is not accessible")
            );

        _logger.LogInformation(
            LogEvents.CategoryReadSuccess,
            "Successfully read {category}",
            category.ToString()
        );

        return new Category(category);
    }
}
