using FinBookeAPI.Collections.CategoryCollection;
using FinBookeAPI.Models.CategoryType;
using Moq;

namespace FinBookeAPI.Tests.Mocks.Collections;

public static class MockCategoryCollection
{
    public static Mock<ICategoryCollection> GetMock(List<Category> data)
    {
        var result = new Mock<ICategoryCollection>();
        result.Setup(obj => obj.CreateCategory(It.IsAny<Category>())).Callback<Category>(data.Add);
        result
            .Setup(obj => obj.UpdateCategory(It.IsAny<Category>()))
            .Callback<Category>(input =>
            {
                var idx = data.FindIndex(elem => elem.Id == input.Id);
                if (idx != -1)
                {
                    data[idx] = input;
                }
            });
        result
            .Setup(obj => obj.DeleteCategory(It.IsAny<Category>()))
            .Callback<Category>(input =>
            {
                data.Remove(input);
            });
        result
            .Setup(obj => obj.GetCategory(It.IsAny<Guid>()))
            .ReturnsAsync(
                (Guid id) =>
                {
                    return data.FirstOrDefault(elem => elem.Id == id);
                }
            );
        result
            .Setup(obj => obj.GetCategories(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(
                (IEnumerable<Guid> ids) =>
                {
                    return data.Where(elem => ids.Contains(elem.Id));
                }
            );
        result
            .Setup(obj => obj.ExistCategory(It.IsAny<Guid>()))
            .ReturnsAsync(
                (Guid id) =>
                {
                    return data.Any(elem => elem.Id == id);
                }
            );
        result
            .Setup(obj => obj.ExistCategories(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(
                (IEnumerable<Guid> ids) =>
                {
                    return ids.All(id => data.Any(elem => elem.Id == id));
                }
            );
        result
            .Setup(obj => obj.HasAccess(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(
                (Guid userId, Guid id) =>
                {
                    return data.Any(elem =>
                        elem.Id == id && (elem.UserId == userId || elem.UserId == Guid.Empty)
                    );
                }
            );
        result
            .Setup(obj => obj.HasAccess(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(
                (Guid userId, IEnumerable<Guid> ids) =>
                {
                    return ids.All(id =>
                        data.Any(elem =>
                            elem.Id == id && (elem.UserId == userId || elem.UserId == Guid.Empty)
                        )
                    );
                }
            );
        return result;
    }
}
