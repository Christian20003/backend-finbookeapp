namespace FinBookeAPI.Services.GenHash;

public partial class GenHashService : IGenHashService
{
    public string GenerateAccessCode(int length)
    {
        _logger.LogDebug("Generate a new access code with length {length}", length);
        if (length <= 0)
            throw new ArgumentException("length must be larger than zero", nameof(length));

        var options = _upperCaseLetters + _digits;
        var code = GetRandomString(length, options);
        return new string(code);
    }
}
