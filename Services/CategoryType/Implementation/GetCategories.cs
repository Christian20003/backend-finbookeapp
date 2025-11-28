using FinBookeAPI.AppConfig.Documentation;
using FinBookeAPI.Models.CategoryType;
using FinBookeAPI.Models.Configuration;

namespace FinBookeAPI.Services.CategoryType;

public partial class CategoryService : ICategoryService
{
    public async Task<IEnumerable<CategoryNested>> GetCategories(Guid userId)
    {
        _logger.LogDebug("Read all categories of {id}", userId);

        if (userId == Guid.Empty)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new ArgumentException("UserId is an empty Guid", nameof(userId))
            );

        var data = await _collection.GetCategories(userId);
        var childIds = data.SelectMany(elem => elem.Children);
        var mainCategories = data.Where(elem => !childIds.Contains(elem.Id));
        var subCategories = data.Where(elem => childIds.Contains(elem.Id))
            .ToDictionary(elem => elem.Id);
        var result = new List<CategoryNested>();

        foreach (var category in mainCategories)
        {
            result.Add(TransformCategory(category, subCategories));
        }

        return result;
    }
}
