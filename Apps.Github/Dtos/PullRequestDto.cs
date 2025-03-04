using Octokit;

namespace Apps.Github.Dtos;

public class PullRequestDto(PullRequest source)
{
    public string Id { get; set; } = source.Id.ToString();

    public string Number { get; set; } = source.Number.ToString();

    public string Title { get; set; } = source.Title;

    public string Body { get; set; } = source.Body;

    public string UserLogin { get; set; } = source.User.Login;

    public string Url { get; set; } = source.HtmlUrl;
}