using FinBookeAPI.Models.Token;
using Microsoft.EntityFrameworkCore;

namespace FinBookeAPI.Collections.TokenCollection;

public interface ITokenCollection
{
    /// <summary>
    /// This method adds the provided token into the collection.
    /// </summary>
    /// <param name="token">
    /// The token that should be added.
    /// </param>
    /// <exception cref="DbUpdateException">
    /// If the insertion operation in the database failed.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the database operation has been canceled.
    /// </exception>
    public Task Add(JwtToken token);

    /// <summary>
    /// This method proofs if a token is stored in the token collection.
    /// </summary>
    /// <param name="token">
    /// The token value
    /// </param>
    /// <returns>
    /// <c>true</c> if the provided token value is already stored, otherwise <c>false</c>.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// If the database operation has been canceled.
    /// </exception>
    public Task<bool> Contains(string token);

    /// <summary>
    /// This method removes all tokens from the collection that have been expired.
    /// </summary>
    /// <exception cref="DbUpdateException">
    /// If the deletion operation in the database failed.
    /// </exception>
    /// <exception cref="DbUpdateConcurrencyException">
    /// If tokens to be deleted have been modified.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the database operation has been canceled.
    /// </exception>
    public Task Delete();
}
