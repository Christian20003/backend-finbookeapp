namespace FinBookeAPI.Tests.Email;

public partial class EmailUnitTests
{
    [Fact]
    public void Should_FailToSendEmail_WhenHostIsNull()
    {
        _options.Host = null;

        Assert.Throws<ApplicationException>(() => _service.Send(_email, _subject, _body));
    }

    [Fact]
    public void Should_FailToSendEmail_WhenAddressIsNull()
    {
        _options.Address = null;

        Assert.Throws<ApplicationException>(() => _service.Send(_email, _subject, _body));
    }

    [Fact]
    public void Should_FailToSendEmail_WhenAddressIsInvalid()
    {
        _options.Address = "invalidEmailAddress";

        Assert.Throws<ApplicationException>(() => _service.Send(_email, _subject, _body));
    }

    [Fact]
    public void Should_FailToSendEmail_WhenPortIsNegativ()
    {
        _options.Port = -12;

        Assert.Throws<ApplicationException>(() => _service.Send(_email, _subject, _body));
    }

    [Fact]
    public void Should_FailToSendEmail_WhenPortIsTooLarge()
    {
        _options.Port = 600000;

        Assert.Throws<ApplicationException>(() => _service.Send(_email, _subject, _body));
    }
}
