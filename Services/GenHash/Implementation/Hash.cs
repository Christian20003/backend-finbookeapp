using System.Security.Cryptography;

namespace FinBookeAPI.Services.GenHash;

public partial class GenHashService : IGenHashService
{
    public string Hash(string content)
    {
        _logger.LogDebug("Generate a new hash value");
        byte[] salt = RandomNumberGenerator.GetBytes(_saltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(content, salt, _iterations, _algorithm, _hashSize);
        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
    }

    public bool IsHash(string content, string contentHash)
    {
        _logger.LogDebug("Verify a hash value: {value}", contentHash);
        string[] parts = contentHash.Split("-");
        if (parts.Length != 2)
        {
            throw new ArgumentException("The hash value is not valid", nameof(contentHash));
        }

        byte[] hash = Convert.FromHexString(parts[0]);
        byte[] salt = Convert.FromHexString(parts[1]);

        byte[] toVerify = Rfc2898DeriveBytes.Pbkdf2(
            content,
            salt,
            _iterations,
            _algorithm,
            _hashSize
        );

        return hash.SequenceEqual(toVerify);
    }
}
