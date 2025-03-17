using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.DTO.Error;

public class ErrorResponse
{
    public int Status { get; set; }

    public string Message { get; set; } = "";

    public ErrorCodes? Code { get; set; } = null;
}
