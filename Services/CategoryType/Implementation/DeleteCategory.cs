using FinBookeAPI.AppConfig.Documentation;
using FinBookeAPI.Models.CategoryType;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Services.CategoryType;

public partial class CategoryService : ICategoryService
{
    public async Task<Category> DeleteCategory(Category category)
    {
        _logger.LogDebug("Delete category: {category}", category.ToString());

        var dbCategory = await _collection.GetCategory(category.Id);
        if (dbCategory is null)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryDeleteFailed,
                new EntityNotFoundException("Category does not exist")
            );
        if (dbCategory.UserId != category.UserId)
            Logging.ThrowAndLogError(
                _logger,
                LogEvents.CategoryDeleteFailed,
                new AuthorizationException("Category is not accessible")
            );
        var categories = await _collection.GetCategories(category.UserId);
        var parent = categories.FirstOrDefault(elem => elem.Children.Contains(category.Id));
        if (parent is not null)
        {
            parent.Children = parent.Children.Where(childId => childId != category.Id);
            _collection.UpdateCategory(parent);
        }

        _collection.DeleteCategory(dbCategory);
        await _collection.SaveChanges();

        _logger.LogInformation(
            LogEvents.CategoryDeleteSuccess,
            "Category {category} has been deleted",
            category.ToString()
        );

        return new Category(dbCategory);
    }
}
