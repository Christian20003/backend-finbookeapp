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
            if (await CycleCheck(id, map))
                return true;
        }
        return false;
    }
}
