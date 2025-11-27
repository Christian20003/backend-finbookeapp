namespace FinBookeAPI.Services.CategoryType;

public partial class CategoryService : ICategoryService
{
    /// <summary>
    /// This method checks if a cycle exists in the connection of sub
    /// categories.
    /// </summary>
    /// <param name="first">
    /// The id of the category where the cycle check should start.
    /// </param>
    /// <param name="categoryIds">
    /// A list of all category ids that are directly or indirectly
    /// connected to the <c>first</c> category.
    /// </param>
    /// <returns>
    /// <c>True</c> if a cycle exist, otherwise <c>false</c>.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// If the reading operation has been canceled.
    /// </exception>
    private async Task<bool> HasCycles(Guid first, IEnumerable<Guid> categoryIds)
    {
        if (categoryIds.Any())
            return false;
        var map = new HashSet<Guid> { first };
        foreach (var id in categoryIds)
        {
            if (await DFS(id, map))
                return true;
        }
        return false;
    }

    private async Task<bool> DFS(Guid current, HashSet<Guid> visited)
    {
        var category = await _collection.GetCategory(current);
        if (category is null)
            return false;
        if (!visited.Add(current))
            return true;
        foreach (var child in category.Children)
        {
            if (await DFS(child, visited))
                return true;
        }
        return false;
    }
}
