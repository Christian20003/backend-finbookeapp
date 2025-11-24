namespace FinBookeAPI.DTO.Error;

public class BadRequestDTO
{
    public string Type { get; set; } = "";

    public string Title { get; set; } = "";

    public int Status { get; set; }

    public Dictionary<string, List<string>> Errors { get; set; } = [];

    public string TraceId { get; set; } = "";
}
