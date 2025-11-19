namespace FinBookeAPI.Tests.SecurityUtility;

public partial class SecurityUtilityUnitTests
{
    [Fact]
    public void Should_FailToGeneratePassword_WhenInvalidLength()
    {
        Assert.Throws<ArgumentException>(() => _service.GeneratePassword(-5));
        Assert.Throws<ArgumentException>(() => _service.GeneratePassword(0));
    }

    [Fact]
    public void Should_ReturnNotEmptyPassword_WhenValidLength()
    {
        var result = _service.GeneratePassword(15);

        Assert.NotEqual("", result);
    }

    [Fact]
    public void Should_ReturnPasswordWithCorrectStructure_WhenValidLength()
    {
        var result = _service.GeneratePassword(15);
        var containsLowerCase = HasLowerCase().IsMatch(result);
        var containsUpperCase = HasUpperCase().IsMatch(result);
        var containsDigits = HasDigit().IsMatch(result);
        var containsSpecialChars = HasSpecialChar().IsMatch(result);

        Assert.True(
            containsLowerCase,
            $"Generated password does not contain any lower case letter: {result}"
        );
        Assert.True(
            containsUpperCase,
            $"Generated password does not contain any upper case letter: {result}"
        );
        Assert.True(containsDigits, $"Generated password does not contain any digit: {result}");
        Assert.True(
            containsSpecialChars,
            $"Generated password does not contain any special character: {result}"
        );
    }

    [Fact]
    public void Should_ReturnPasswordWithRequiredLength_WhenValidLength()
    {
        var result = _service.GeneratePassword(15);

        Assert.Equal(15, result.Length);
    }
}
