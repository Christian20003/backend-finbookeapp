using FinBookeAPI.Collections.CategoryCollection;
using FinBookeAPI.Models.CategoryType;
using Moq;

namespace FinBookeAPI.Tests.Mocks.Collections;

public static class MockCategoryCollection
{
    public static Mock<ICategoryCollection> GetMock(List<Category> data)
    {
        var result = new Mock<ICategoryCollection>();
        result
            .Setup(obj => obj.CreateCategory(It.IsAny<Category>()))
            .Callback<Category>(data.Add)
            .ReturnsAsync(
                (Category input) =>
                {
                    return input;
                }
            );
        result
            .Setup(obj => obj.UpdateCategory(It.IsAny<Category>()))
            .Callback<Category>(input =>
            {
                var idx = data.FindIndex(elem => elem.Id == input.Id);
                if (idx != -1)
                {
                    data[idx] = input;
                }
            })
            .ReturnsAsync(
                (Category input) =>
                {
                    return input;
                }
            );
        result
            .Setup(obj => obj.DeleteCategory(It.IsAny<Category>()))
            .Callback<Category>(input =>
            {
                data.Remove(input);
            })
            .ReturnsAsync(
                (Category input) =>
                {
                    return input;
                }
            );
        result
            .Setup(obj => obj.GetCategory(It.IsAny<Guid>()))
            .ReturnsAsync(
                (Guid id) =>
                {
                    return data.First(elem => elem.Id == id);
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
            .Setup(obj => obj.ExistCategory(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(
                (Guid id, Guid userId) =>
                {
                    return data.Any(elem => elem.Id == id && elem.UserId == userId);
                }
            );
        result
            .Setup(obj => obj.ExistCategories(It.IsAny<IEnumerable<Guid>>(), It.IsAny<Guid>()))
            .ReturnsAsync(
                (IEnumerable<Guid> ids, Guid userId) =>
                {
                    return ids.All(id => data.Any(elem => elem.Id == id && elem.UserId == userId));
                }
            );
        return result;
    }
}
