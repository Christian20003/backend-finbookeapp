using System.Security.Cryptography;

namespace FinBookeAPI.Services.GenHash;

public partial class GenHashService(ILogger<GenHashService> logger) : IGenHashService
{
    private static readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA512;
    private readonly ILogger<GenHashService> _logger = logger;
    private const string _lowerCaseLetters = "abcdefghijklmnopqrstuvwxyz";
    private const string _upperCaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string _digits = "0123456789";
    private const string _specialChars = "!§$%&/()=?{[]}";

    private const int _saltSize = 16;
    private const int _hashSize = 16;
    private const int _iterations = 100000;
}
