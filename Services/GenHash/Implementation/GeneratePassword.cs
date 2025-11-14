using System.Security.Cryptography;

namespace FinBookeAPI.Services.GenHash;

public partial class GenHashService : IGenHashService
{
    public string GeneratePassword(int length)
    {
        _logger.LogDebug("Generating a new password of length {length}", length);
        if (length <= 0)
            throw new ArgumentException("length must be larger than zero", nameof(length));

        var options = new List<string>
        {
            _lowerCaseLetters,
            _upperCaseLetters,
            _digits,
            _specialChars,
        };
        var password = GetRandomString(length, string.Join("", options));
        var positions = new HashSet<int>();

        if (length < 4)
            return new string(password);

        while (positions.Count < 4)
        {
            positions.Add(RandomNumberGenerator.GetInt32(length));
        }

        var index = 0;
        foreach (var position in positions)
        {
            var option = options[index];
            var value = option[RandomNumberGenerator.GetInt32(option.Length)];
            password[position] = value;
            index++;
        }
        return new string(password);
    }
}
