using FinBookeAPI.Models.CategoryType;
using FinBookeAPI.Models.Configuration;

namespace FinBookeAPI.Services.CategoryType;

public partial class CategoryService : ICategoryService
{
    public async Task<Category> CreateMainCategory(Category category)
    {
        _logger.LogDebug("Add new main category {category}", category.ToString());
        await VerifyCategory(category);

        await _collection.CreateCategory(category);
        _logger.LogInformation(
            LogEvents.CategoryOperationSuccess,
            "{category} is added",
            category.ToString()
        );
        return category;
    }
}
