using FinBookeAPI.AppConfig.Database;
using FinBookeAPI.Models.CategoryType;
using Microsoft.EntityFrameworkCore;

namespace FinBookeAPI.Collections.CategoryCollection;

public class CategoryCollection(DataDbContext context) : ICategoryCollection
{
    private readonly DataDbContext _dbContext = context;

    public async Task CreateCategory(Category category)
    {
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateCategory(Category category)
    {
        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCategory(Category category)
    {
        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Category?> GetCategory(Guid categoryId)
    {
        return await _dbContext.Categories.FirstOrDefaultAsync(category =>
            category.Id == categoryId
        );
    }

    public async Task<IEnumerable<Category>> GetCategories(Guid userId)
    {
        return await _dbContext
            .Categories.Where(category => category.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetCategories(IEnumerable<Guid> categoryIds)
    {
        return await _dbContext
            .Categories.Where(category => categoryIds.Contains(category.Id))
            .ToListAsync();
    }

    public async Task<bool> ExistCategory(Guid categoryId)
    {
        return await _dbContext.Categories.AnyAsync(category => category.Id == categoryId);
    }

    public async Task<bool> ExistCategories(IEnumerable<Guid> categoryIds)
    {
        if (!categoryIds.Any())
            return true;
        var list = await _dbContext
            .Categories.Where(category => categoryIds.Contains(category.Id))
            .Select(category => category.Id)
            .ToListAsync();
        return categoryIds.All(list.Contains);
    }

    public async Task<bool> HasAccess(Guid userId, Guid categoryId)
    {
        return await _dbContext.Categories.AnyAsync(category =>
            category.Id == categoryId && category.UserId == userId
        );
    }

    public async Task<bool> HasAccess(Guid userId, IEnumerable<Guid> categoryIds)
    {
        if (!categoryIds.Any())
            return true;
        var list = await _dbContext
            .Categories.Where(category =>
                categoryIds.Contains(category.Id) && category.UserId == userId
            )
            .Select(category => category.Id)
            .ToListAsync();
        return categoryIds.All(list.Contains);
    }
}
