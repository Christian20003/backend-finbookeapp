namespace FinBookeAPI.Models.Configuration;

public static class FileStorage
{
    /// <summary>
    /// This method returns the path where the receipts of the
    /// specified user are stored.
    /// </summary>
    /// <param name="userId">
    /// The user id.
    /// </param>
    /// <returns>
    /// The absolute path where receipt files are stored.
    /// </returns>
    public static string GetReceiptPath(Guid userId)
    {
        return $"documents/{userId}/receipts/";
    }

    /// <summary>
    /// This method returns the path where the bank statements
    /// of a specified user are stored.
    /// </summary>
    /// <param name="userId">
    /// The user id.
    /// </param>
    /// <returns>
    /// The absolute path where bank statement files are stored.
    /// </returns>
    public static string GetBankStatementPath(Guid userId)
    {
        return $"documents/{userId}/statements/";
    }
}
