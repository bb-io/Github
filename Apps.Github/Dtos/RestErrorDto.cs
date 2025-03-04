namespace Apps.GitHub.Dtos;

public class RestErrorDto
{
    public string Message { get; set; } = string.Empty;
    
    public string DocumentationUrl { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
