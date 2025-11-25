using FinBookeAPI.Models.CategoryType;

namespace FinBookeAPI.Tests.Records;

public static class CategoryRecord
{
    public static Category GetObject()
    {
        return new Category
        {
            Name = "Living",
            Color = "rgb(66,55,88)",
            UserId = new Guid(),
        };
    }

    public static List<Category> GetObjects()
    {
        var userId = new Guid();
        var firstChild = new Guid();
        var secondChild = new Guid();
        return
        [
            new Category
            {
                UserId = new Guid(),
                Name = "Hobbys",
                Color = "rgb(77,44,33)",
            },
            new Category
            {
                UserId = userId,
                Name = "apartment",
                Color = "rgb(62,69,32)",
                Children = [firstChild, secondChild],
            },
            new Category
            {
                UserId = userId,
                Name = "electricity",
                Id = firstChild,
                Color = "rgb(235,64,14)",
            },
            new Category
            {
                UserId = userId,
                Name = "heating",
                Id = secondChild,
                Color = "rgb(235,164,14)",
            },
        ];
    }
}
