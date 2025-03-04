namespace Apps.GitHub.Dtos;

public class GithubErrorException(int code, string message) : ArgumentException(message)
{
    public int ErrorCode { get; set; } = code;
}