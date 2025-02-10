namespace Apps.GitHub.Dtos;

public class GithubErrorException : ArgumentException
{
    public int ErrorCode { get; set; }
    public GithubErrorException(int code, string message) : base(message)
    {
        ErrorCode = code;
    }
}