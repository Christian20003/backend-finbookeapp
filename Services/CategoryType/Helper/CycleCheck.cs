namespace FinBookeAPI.Services.CategoryType;

public partial class CategoryService : ICategoryService
{
    /// <summary>
    /// This method executes a depth-first-search (recursively) in order to detect
    /// any cycles in the category relationships.
    /// </summary>
    /// <param name="current">
    /// The id of the category which should be visited.
    /// </param>
    /// <param name="visited">
    /// A hash map that stores all visited category ids.
    /// </param>
    /// <returns>
    /// <c>True</c> if a cycle exist, otherwise <c>false</c>.
    /// </returns>
    private async Task<bool> CycleCheck(Guid current, HashSet<Guid> visited)
    {
        var category = await _collection.GetCategory(current);
        if (category is null)
            return false;
        if (!visited.Add(current))
            return true;
        foreach (var child in category.Children)
        {
            if (await CycleCheck(child, visited))
                return true;
        }

        return false;
    }
}
