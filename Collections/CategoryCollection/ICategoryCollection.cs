using FinBookeAPI.Models.CategoryType;
using Microsoft.EntityFrameworkCore;

namespace FinBookeAPI.Collections.CategoryCollection;

public interface ICategoryCollection
{
    /// <summary>
    /// This method creates a new category object in the database.
    /// </summary>
    /// <param name="category">
    /// The category object that should be added to the database.
    /// </param>
    /// <exception cref="DbUpdateException">
    /// If the category collection could not be updated.
    /// </exception>
    /// <exception cref="DbUpdateConcurrencyException">
    /// If the category collection could not be updated due to concurrency issues.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the insertion operation has been canceled.
    /// </exception>
    public Task CreateCategory(Category category);

    /// <summary>
    /// This method updates an existing category in the database. Ensure
    /// that the updated entity is stored in the database, otherwise
    /// an exception is thrown.
    /// </summary>
    /// <param name="category">
    /// The category from the database with updated properties.
    /// </param>
    /// <exception cref="DbUpdateException">
    /// If the category collection could not be updated.
    /// </exception>
    /// <exception cref="DbUpdateConcurrencyException">
    /// If the category collection could not be updated due to concurrency issues.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the update operation has been canceled.
    /// </exception>
    public Task UpdateCategory(Category category);

    /// <summary>
    /// This method deletes an category from the database. Ensure
    /// that the entity is stored in the database, otherwise
    /// an exception is thrown.
    /// </summary>
    /// <param name="category">
    /// The category from the database that should be deleted.
    /// </param>
    /// <exception cref="DbUpdateException">
    /// If the category collection could not be updated.
    /// </exception>
    /// <exception cref="DbUpdateConcurrencyException">
    /// If the category collection could not be updated due to concurrency issues.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the delete operation has been canceled.
    /// </exception>
    public Task DeleteCategory(Category category);

    /// <summary>
    /// This method returns the category with the provided id from the database.
    /// </summary>
    /// <param name="categoryId">
    /// The id of the category that should be returned.
    /// </param>
    /// <returns>
    /// The requested category if it exists, othwerwise <c>null</c>.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// If the reading operation has been canceled.
    /// </exception>
    public Task<Category?> GetCategory(Guid categoryId);

    /// <summary>
    /// This method returns a list of categories with the provided ids from the database.
    /// </summary>
    /// <param name="categoryIds">
    /// The ids of the categories that should be returned.
    /// </param>
    /// <returns>
    /// The requested categories.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// If the reading operation has been canceled.
    /// </exception>
    public Task<IEnumerable<Category>> GetCategories(IEnumerable<Guid> categoryIds);

    /// <summary>
    /// This method proofs if the given category id is stored in the database.
    /// </summary>
    /// <param name="categoryId">
    /// The id of the category that should be checked.
    /// </param>
    /// <returns>
    /// <c>True</c> if the category exist in the database, otherwise <c>false</c>.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// If the reading operation has been canceled.
    /// </exception>
    public Task<bool> ExistCategory(Guid categoryId);

    /// <summary>
    /// This method proofs if the given category ids are stored in the database.
    /// </summary>
    /// <param name="categoryIds">
    /// The ids of the categories that should be checked.
    /// </param>
    /// <returns>
    /// <c>True</c> if all categories exist in the database, otherwise <c>false</c>
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// If the reading operation has been canceled.
    /// </exception>
    public Task<bool> ExistCategories(IEnumerable<Guid> categoryIds);

    /// <summary>
    /// This method proofs if the user has access to the category.
    /// It will also return <c>false</c> if the category does not
    /// exist.
    /// </summary>
    /// <param name="userId">
    /// The id of the user.
    /// </param>
    /// <param name="categoryId">
    /// The id of the category.
    /// </param>
    /// <returns>
    /// <c>True</c> if the user has access, otherwise <c>false</c>.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// If the reading operation has been canceled.
    /// </exception>
    public Task<bool> HasAccess(Guid userId, Guid categoryId);

    /// <summary>
    /// This method proofs if the user has access to the categories.
    /// It will also return <c>false</c> if a single category does not
    /// exist.
    /// </summary>
    /// <param name="userId">
    /// The id of the user.
    /// </param>
    /// <param name="categoryIds">
    /// The ids of the categories.
    /// </param>
    /// <returns>
    /// <c>True</c> if the user has access, otherwise <c>false</c>.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// If the reading operation has been canceled.
    /// </exception>
    public Task<bool> HasAccess(Guid userId, IEnumerable<Guid> categoryIds);
}
