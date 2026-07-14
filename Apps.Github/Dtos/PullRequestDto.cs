using Octokit;

namespace Apps.Github.Dtos;

public class PullRequestDto(PullRequest source)
{
    public string Id { get; set; } = source.Id.ToString();

    public string Number { get; set; } = source.Number.ToString();

    public string Title { get; set; } = source.Title;

    public string Body { get; set; } = source.Body ?? string.Empty;

    public string UserLogin { get; set; } = source.User.Login;

    public string Url { get; set; } = source.HtmlUrl;

    public string State { get; set; } = source.State.StringValue;

    public bool Draft { get; set; } = source.Draft;

    public string HeadSha { get; set; } = source.Head.Sha;

    public string HeadRef { get; set; } = source.Head.Ref;

    public string BaseSha { get; set; } = source.Base.Sha;

    public string BaseRef { get; set; } = source.Base.Ref;

    public string? MergeableState { get; set; } = source.MergeableState?.Value.ToString();

    public int ChangedFiles { get; set; } = source.ChangedFiles;

    public int Additions { get; set; } = source.Additions;

    public int Deletions { get; set; } = source.Deletions;
}
