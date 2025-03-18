using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.DTO.Error;

/// <summary>
/// This class represents an error response.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// The type of the error (e.g. a specific exception type)
    /// </summary>
    public string Type { get; set; } = "";

    /// <summary>
    /// The title of this error.
    /// </summary>
    public string Title { get; set; } = "";

    /// <summary>
    /// A detailed description of the error.
    /// </summary>
    public string Detail { get; set; } = "";

    /// <summary>
    /// The http status code of the error.
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// The error code which gives further inside about the error source.
    /// </summary>
    public ErrorCodes? Code { get; set; } = null;

    // The uri where this error occurred.
    public string Instance { get; set; } = "";

    public List<string>? InvalidProps { get; set; } = null;
}
