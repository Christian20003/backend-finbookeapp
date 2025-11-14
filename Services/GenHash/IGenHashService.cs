namespace FinBookeAPI.Services.GenHash;

public interface IGenHashService
{
    /// <summary>
    /// This method generates a new random password.
    /// </summary>
    /// <param name="length">
    /// The length of the password.
    /// </param>
    /// <returns>
    /// The generated password.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the provided length is negativ or zero.
    /// </exception>
    public string GeneratePassword(int length);

    /// <summary>
    /// This method generates a new random access code.
    /// </summary>
    /// <param name="length">
    /// The length of the access code.
    /// </param>
    /// <returns>
    /// A random generated access code.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the provided length is negativ or zero.
    /// </exception>
    public string GenerateAccessCode(int length);

    /// <summary>
    /// This method generates a hash from the provided content.
    /// </summary>
    /// <param name="content">
    /// The content that should be hashed.
    /// </param>
    /// <returns>
    /// The hash value.
    /// </returns>
    public string Hash(string content);

    /// <summary>
    /// This method proofs if the provided hash value is equal to the hash
    /// produced with the provided content.
    /// </summary>
    /// <param name="content">
    /// The content that should be hashed and verified.
    /// </param>
    /// <param name="contentHash">
    /// The hash value that is the original hash of the content.
    /// </param>
    /// <returns>
    /// <c>True</c> if the content produces the same hash, otherwise <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the provided hash is not valid.
    /// </exception>
    public bool IsHash(string content, string contentHash);
}
