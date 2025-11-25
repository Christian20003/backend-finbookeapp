using FinBookeAPI.Models.CategoryType;

namespace FinBookeAPI.Services.CategoryType;

public interface ICategoryService
{
    public Task<Category> CreateMainCategory(Category category);
}
