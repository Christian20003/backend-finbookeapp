using System.Text.RegularExpressions;
using FinBookeAPI.Services.SecurityUtility;
using Moq;

namespace FinBookeAPI.Tests.SecurityUtility;

public partial class SecurityUtilityUnitTests
{
    private readonly SecurityUtilityService _service;

    [GeneratedRegex("[a-z]")]
    private static partial Regex HasLowerCase();

    [GeneratedRegex("[A-Z]")]
    private static partial Regex HasUpperCase();

    [GeneratedRegex("[0-9]")]
    private static partial Regex HasDigit();

    [GeneratedRegex("[!§$%&/\\(\\)=?\\{\\[\\]\\}]")]
    private static partial Regex HasSpecialChar();

    public SecurityUtilityUnitTests()
    {
        var logger = new Mock<ILogger<SecurityUtilityService>>();
        _service = new SecurityUtilityService(logger.Object);
    }
}
