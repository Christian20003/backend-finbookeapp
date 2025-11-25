using FinBookeAPI.Models.CategoryType;
using Microsoft.EntityFrameworkCore;

namespace FinBookeAPI.Collections.CategoryCollection;

public interface ICategoryCollection
{
    /// <summary>
    /// This method creates a new category object in the database.
    /// </summary>
    /// <param name="category">The category object that should be added to the database</param>
    /// <returns>The category that has been added</returns>
    /// <exception cref="DbUpdateException">
    /// If the category collection could not be updated.
    /// </exception>
    /// <exception cref="DbUpdateConcurrencyException">
    /// If the category collection could not be updated due to concurrency issues.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the insertion operation has been canceled.
    /// </exception>
    public Task<Category> CreateCategory(Category category);

    /// <summary>
    /// This method updates an existing category based on its id in the database.
    /// If the provided category is not found in the database, it will be created.
    /// </summary>
    /// <param name="category">The updated category</param>
    /// <returns>The updated category</returns>
    /// <exception cref="DbUpdateException">
    /// If the category collection could not be updated.
    /// </exception>
    /// <exception cref="DbUpdateConcurrencyException">
    /// If the category collection could not be updated due to concurrency issues.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the update operation has been canceled.
    /// </exception>
    public Task<Category> UpdateCategory(Category category);

    /// <summary>
    /// This method deletes an category from the database.
    /// </summary>
    /// <param name="category">The category that should be deleted</param>
    /// <returns>The deleted category if it exists, otherwise <c>null</c></returns>
    /// <exception cref="DbUpdateException">
    /// If the category collection could not be updated.
    /// </exception>
    /// <exception cref="DbUpdateConcurrencyException">
    /// If the category collection could not be updated due to concurrency issues.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the delete operation has been canceled.
    /// </exception>
    public Task<Category?> DeleteCategory(Category category);

    /// <summary>
    /// This method returns the category with the provided id from the database.
    /// </summary>
    /// <param name="id">The id of the category that should be returned</param>
    /// <returns>The requested category if it exists, othwerwise <c>null</c></returns>
    /// <exception cref="OperationCanceledException">
    /// If the reading operation has been canceled.
    /// </exception>
    public Task<Category?> GetCategory(Guid id);

    /// <summary>
    /// This method returns a list of categories with the provided ids from the database.
    /// </summary>
    /// <param name="ids">The ids of the categories that should be returned</param>
    /// <returns>The requested categories</returns>
    /// <exception cref="OperationCanceledException">
    /// If the reading operation has been canceled.
    /// </exception>
    public Task<IEnumerable<Category>> GetCategories(IEnumerable<Guid> ids);

    /// <summary>
    /// This method proofs if the given category id is stored in the database and
    /// if the provided user has access to this category.
    /// </summary>
    /// <param name="id">The id of the category that should be checked.</param>
    /// <param name="userId">The id of the user.</param>
    /// <returns>
    /// <c>True</c> if the category exist in the database and the user
    /// owns this category, otherwise <c>false</c>.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// If the reading operation has been canceled.
    /// </exception>
    public Task<bool> ExistCategory(Guid id, Guid userId);

    /// <summary>
    /// This method proofs if the given category ids are stored in the database and
    /// if the provided user has access to these categories.
    /// </summary>
    /// <param name="ids">The ids of the categories that should be checked.</param>
    /// <param name="userId">The id of the user.</param>
    /// <returns><c>True</c> if all categories exist in the database and the user
    /// owns all categories, otherwise <c>false</c></returns>
    /// <exception cref="OperationCanceledException">
    /// If the reading operation has been canceled.
    /// </exception>
    public Task<bool> ExistCategories(IEnumerable<Guid> ids, Guid userId);
}
