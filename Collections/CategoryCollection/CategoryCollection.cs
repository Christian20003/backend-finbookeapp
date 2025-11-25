using FinBookeAPI.AppConfig.Database;
using FinBookeAPI.Models.CategoryType;
using Microsoft.EntityFrameworkCore;

namespace FinBookeAPI.Collections.CategoryCollection;

public class CategoryCollection(DataDbContext context) : ICategoryCollection
{
    private readonly DataDbContext _dbContext = context;

    public async Task<Category> CreateCategory(Category category)
    {
        var entity = await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();
        return entity.Entity;
    }

    public async Task<Category> UpdateCategory(Category category)
    {
        var entity = _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync();
        return entity.Entity;
    }

    public async Task<Category?> DeleteCategory(Category category)
    {
        var entity = _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();
        return entity.Entity;
    }

    public async Task<Category?> GetCategory(Guid id)
    {
        var entity = await _dbContext.Categories.FirstOrDefaultAsync(category => category.Id == id);
        return entity;
    }

    public async Task<IEnumerable<Category>> GetCategories(IEnumerable<Guid> ids)
    {
        return await _dbContext
            .Categories.Where(category => ids.Contains(category.Id))
            .ToListAsync();
    }

    public async Task<bool> ExistCategory(Guid id, Guid userId)
    {
        return await _dbContext.Categories.AnyAsync(category =>
            category.Id == id && category.UserId == userId
        );
    }

    public async Task<bool> ExistCategories(IEnumerable<Guid> ids, Guid userId)
    {
        var list = await _dbContext
            .Categories.Where(category => ids.Contains(category.Id) && category.UserId == userId)
            .Select(category => category.Id)
            .ToListAsync();
        return ids.All(list.Contains);
    }
}
