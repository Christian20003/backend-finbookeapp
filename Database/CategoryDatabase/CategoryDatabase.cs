using FinBookeAPI.AppConfig.Database;
using FinBookeAPI.Models.CategoryModels;
using Microsoft.EntityFrameworkCore;

namespace FinBookeAPI.Database.CategoryDatabase;

public class CategoryDatabase(DataDbContext context) : ICategoryDatabase
{
    private readonly DataDbContext _dbContext = context;

    public async Task<Category> CreateCategory(Category category)
    {
        var entity = await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();
        return entity.Entity;
    }

    public async Task<Category?> DeleteCategory(Guid id)
    {
        var category = await GetCategory(id);
        if (category == null)
        {
            return category;
        }
        var entity = _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();
        return entity.Entity;
    }

    public async Task<Category?> GetCategory(Guid id)
    {
        var entity = await _dbContext.Categories.FirstOrDefaultAsync(category => category.Id == id);
        return entity;
    }

    public async Task<Category> UpdateCategory(Category category)
    {
        var entity = _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync();
        return entity.Entity;
    }
}
