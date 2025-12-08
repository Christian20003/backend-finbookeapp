using FinBookeAPI.AppConfig.Documentation;
using FinBookeAPI.Models.CategoryType;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Services.CategoryType;

public partial class CategoryService : ICategoryService
{
    public async Task<IEnumerable<Category>> UpdateCategory(Category category)
    {
        _logger.LogDebug("Update category: {catgeory}", category.ToString());

        // Check if database store category
        var dbCategory = await _collection.GetCategory(category.Id);
        if (dbCategory is null)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryUpdateFailed,
                new EntityNotFoundException($"Category does not exist in database")
            );
        if (dbCategory.UserId != category.UserId)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryUpdateFailed,
                new AuthorizationException($"Category is not accessible")
            );
        var result = new List<Category> { category };
        // Check if properties are valid
        await VerifyCategory(category);

        // Set new properties if they have changed
        if (dbCategory.Name != category.Name)
            dbCategory.Name = category.Name;
        if (dbCategory.Color != category.Color)
            dbCategory.Color = category.Color;
        if (dbCategory.Limit != category.Limit)
        {
            if (category.Limit is null)
                dbCategory.Limit = null;
            else if (dbCategory.Limit is null)
                dbCategory.Limit = category.Limit;
            else
            {
                dbCategory.Limit.Amount = category.Limit.Amount;
                dbCategory.Limit.PeriodDays = category.Limit.PeriodDays;
                dbCategory.ModifiedAt = DateTime.UtcNow;
            }
        }

        if (!dbCategory.Children.Equals(category.Children))
        {
            // Update old parents of new childs
            var addedChilds = category.Children.Where(childId =>
                !dbCategory.Children.Contains(childId)
            );
            foreach (var child in addedChilds)
            {
                // Remove their id from children list of old parent
                var parent = await _collection.HasParent(child, category.UserId);
                if (parent is null)
                    continue;
                parent.Children = [.. parent.Children.Where(childId => childId != child)];
                parent.ModifiedAt = DateTime.UtcNow;
                if (result.Any(elem => elem.Id == parent.Id))
                    result = [.. result.Where(elem => elem.Id != parent.Id)];
                result.Add(new Category(parent));
            }
            dbCategory.Children = category.Children;

            if (await VerifyAfterCycle(category.Id, category.Children))
                Logging.ThrowAndLogWarning(
                    _logger,
                    LogEvents.CategoryUpdateFailed,
                    new ArgumentException(
                        $"Category children of {category.Id} produce a cyclic relationship",
                        nameof(category)
                    )
                );
        }

        if (!category.Equals(dbCategory))
            dbCategory.ModifiedAt = category.ModifiedAt;

        _collection.UpdateCategory(dbCategory);
        await _collection.SaveChanges();

        _logger.LogInformation(
            LogEvents.CategoryUpdateSuccess,
            "{category} has been updated successfully",
            dbCategory.ToString()
        );

        return result;
    }
}
