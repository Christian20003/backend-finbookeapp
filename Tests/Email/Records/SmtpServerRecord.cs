using FinBookeAPI.Models.Configuration;

namespace FinBookeAPI.Tests.Email.Records;

public static class SmtpServerRecord
{
    public static SmtpServer GetObject()
    {
        return new SmtpServer
        {
            Host = "http://host",
            Username = "max",
            Password = "1234",
            Address = "max.mustermann@gmx.de",
        };
    }
}
