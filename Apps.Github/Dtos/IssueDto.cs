using Octokit;

namespace Apps.Github.Dtos;

public class IssueDto(Issue source)
{
    public string Title { get; set; } = source.Title;

    public string Body { get; set; } = source.Body;

    public string UserLogin { get; set; } = source.User.Login;

    public string Url { get; set; } = source.HtmlUrl;
}