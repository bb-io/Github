using Apps.Github.Models.PullRequest.Payloads;

namespace Apps.Github.Dtos;

public class CompareCommitDto(CompareCommitPayload source)
{
    public string Id { get; set; } = source.Sha ?? string.Empty;

    public string Url { get; set; } = source.Url ?? string.Empty;

    public string Message { get; set; } = source.Commit?.Message ?? string.Empty;

    public string AuthorLogin { get; set; } = source.Author?.Login ?? string.Empty;
}
