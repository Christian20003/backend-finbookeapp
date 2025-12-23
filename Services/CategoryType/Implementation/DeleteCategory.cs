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

        var entity = await VerifyCategoryAccess(categoryId, userId);
        var parent = await _collection.GetCategory(category =>
            category.Children.Contains(categoryId)
        );
        if (parent is not null)
        {
            if (parent.UserId != userId)
                Logging.ThrowAndLogWarning(
                    _logger,
                    LogEvents.CategoryOperationFailed,
                    new AuthorizationException("Category parent is not accessible")
                );
            parent.Children = parent.Children.Where(childId => childId != categoryId);
            _collection.UpdateCategory(parent);
        }

        _collection.DeleteCategory(entity);
        await _collection.SaveChanges();

        _logger.LogInformation(
            LogEvents.CategoryDeleteSuccess,
            "Category {category} has been deleted",
            entity.ToString()
        );

        return new Category(entity);
    }
}
