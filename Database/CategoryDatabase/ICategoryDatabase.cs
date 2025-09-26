using FinBookeAPI.Models.CategoryModels;
using Microsoft.EntityFrameworkCore;

namespace FinBookeAPI.Database.CategoryDatabase;

public interface ICategoryDatabase
{
    /// <summary>
    /// This function creates a new ategory object in the database.
    /// </summary>
    /// <param name="category">The category object that should be added to the database</param>
    /// <returns>The category that has been added</returns>
    /// <exception cref="DbUpdateException">
    /// If the database has failed to update the database collection.
    /// </exception>
    /// <exception cref="DbUpdateConcurrencyException">
    /// .If the database operation has failed due to concurrency issues.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the creation process has failed.
    /// </exception>
    public Task<Category> CreateCategory(Category category);

    /// <summary>
    /// This function updates an existing category based on its id in the database.
    /// If the provided category is not found in the database, it will be created.
    /// </summary>
    /// <param name="category">The updated category</param>
    /// <returns>The updated category</returns>
    /// <exception cref="DbUpdateException">
    /// If the database has failed to update the database collection.
    /// </exception>
    /// <exception cref="DbUpdateConcurrencyException">
    /// .If the database operation has failed due to concurrency issues.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the creation process has failed.
    /// </exception>
    public Task<Category> UpdateCategory(Category category);

    /// <summary>
    /// This function deletes an category from the database.
    /// </summary>
    /// <param name="id">The id of the category that should be deleted</param>
    /// <returns>The deleted category if it exists, otherwise <c>null</c></returns>
    /// <exception cref="DbUpdateException">
    /// If the database has failed to update the database collection.
    /// </exception>
    /// <exception cref="DbUpdateConcurrencyException">
    /// .If the database operation has failed due to concurrency issues.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the creation process has failed.
    /// </exception>
    public Task<Category?> DeleteCategory(Guid id);

    /// <summary>
    /// This function returns the category with the provided id from the database.
    /// </summary>
    /// <param name="id">The id of the category that should be returned</param>
    /// <returns>The requested category if it exists, othwerwise <c>null</c></returns>
    /// <exception cref="DbUpdateException">
    /// If the database has failed to update the database collection.
    /// </exception>
    /// <exception cref="DbUpdateConcurrencyException">
    /// .If the database operation has failed due to concurrency issues.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the creation process has failed.
    /// </exception>
    public Task<Category?> GetCategory(Guid id);
}
