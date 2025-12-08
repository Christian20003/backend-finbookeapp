using FinBookeAPI.AppConfig.Documentation;
using FinBookeAPI.Models.CategoryType;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Services.CategoryType;

public partial class CategoryService : ICategoryService
{
    public async Task<Category> DeleteCategory(Guid categoryId, Guid userId)
    {
        _logger.LogDebug("Delete category: {category}", categoryId);

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

        var dbCategory = await _collection.GetCategory(categoryId);
        if (dbCategory is null)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryDeleteFailed,
                new EntityNotFoundException("Category does not exist")
            );
        if (dbCategory.UserId != userId)
            Logging.ThrowAndLogError(
                _logger,
                LogEvents.CategoryDeleteFailed,
                new AuthorizationException("Category is not accessible")
            );
        var categories = await _collection.GetCategories(userId);
        var parent = categories.FirstOrDefault(elem => elem.Children.Contains(categoryId));
        if (parent is not null)
        {
            parent.Children = parent.Children.Where(childId => childId != categoryId);
            _collection.UpdateCategory(parent);
        }

        _collection.DeleteCategory(dbCategory);
        await _collection.SaveChanges();

        _logger.LogInformation(
            LogEvents.CategoryDeleteSuccess,
            "Category {category} has been deleted",
            dbCategory.ToString()
        );

        return new Category(dbCategory);
    }
}
