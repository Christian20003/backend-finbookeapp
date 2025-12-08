using FinBookeAPI.Models.CategoryType;
using FinBookeAPI.Models.Configuration;

namespace FinBookeAPI.Services.CategoryType;

public partial class CategoryService : ICategoryService
{
    public async Task<Category> CreateCategory(Category category)
    {
        _logger.LogDebug("Add new category {category}", category.ToString());
        await VerifyCategory(category);
        await _collection.CreateCategory(category);
        await _collection.SaveChanges();

        _logger.LogInformation(
            LogEvents.CategoryInsertSuccess,
            "{category} is added",
            category.ToString()
        );
        return new Category(category);
    }
}
