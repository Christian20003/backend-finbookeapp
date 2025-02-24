namespace FinBookeAPI.Models.Authentication.Interfaces;

public interface IToken
{
    // The token value
    public string Value { get; }

    // The time after this token expires
    public long Expires { get; }
}
