using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Services.Email;
using FinBookeAPI.Tests.Email.Records;
using Microsoft.Extensions.Options;
using Moq;

namespace FinBookeAPI.Tests.Email;

public class SendUnitTest
{
    private readonly EmailService _service;
    private readonly SmtpServer _options = SmtpServerRecord.GetObject();
    private const string _email = "alice@gmx.com";
    private const string _subject = "This is a test";
    private const string _body = "Hello World!";

    public SendUnitTest()
    {
        var logger = new Mock<ILogger<EmailService>>();
        var settings = new Mock<IOptions<SmtpServer>>();
        settings.Setup(obj => obj.Value).Returns(_options);
        _service = new EmailService(settings.Object, logger.Object);
    }

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
