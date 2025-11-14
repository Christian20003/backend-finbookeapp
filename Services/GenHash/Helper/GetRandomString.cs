using System.Security.Cryptography;

namespace FinBookeAPI.Services.GenHash;

public partial class GenHashService : IGenHashService
{
    /// <summary>
    /// This method generates a char array containing random characters.
    /// </summary>
    /// <param name="length">
    /// The length of the array.
    /// </param>
    /// <param name="options">
    /// All possible characters that can be used.
    /// </param>
    /// <returns>
    /// A char array containing random selected characters
    /// </returns>
    private static char[] GetRandomString(int length, string options)
    {
        char[] result = new char[length];
        for (int i = 0; i < length; i++)
        {
            var index = RandomNumberGenerator.GetInt32(options.Length);
            result[i] = options[index];
        }
        return result;
    }
}
