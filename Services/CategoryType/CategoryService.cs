using FinBookeAPI.Collections.CategoryCollection;

namespace FinBookeAPI.Services.CategoryType;

public partial class CategoryService(
    ICategoryCollection collection,
    ILogger<CategoryService> logger
) : ICategoryService
{
    private readonly ICategoryCollection _collection = collection;
    private readonly ILogger<CategoryService> _logger = logger;
}
