using FinBookeAPI.Models.CategoryType;
using FinBookeAPI.Models.Configuration;

namespace FinBookeAPI.Services.CategoryType;

public partial class CategoryService : ICategoryService
{
    public async Task<Category> CreateMainCategory(Category category)
    {
        _logger.LogDebug("Add new main category {name}", category.Name);
        VerifyCategory(category);
        await VerifyCategoryChilds(category.UserId, category.Children);

        var result = await _collection.CreateCategory(category);
        _logger.LogInformation(
            LogEvents.CategoryOperationSuccess,
            "A new category {Name} is added: {Id}",
            category.Name,
            category.Id
        );
        return result;
    }
}
