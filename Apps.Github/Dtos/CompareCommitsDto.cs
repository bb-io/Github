using Apps.Github.Models.PullRequest.Payloads;
using Apps.GitHub.Dtos;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Github.Dtos;

public class CompareCommitsDto(CompareCommitsPayload source)
{
    public string Status { get; set; } = source.Status ?? string.Empty;

    [Display("Ahead by")]
    public int AheadBy { get; set; } = source.AheadBy;

    [Display("Behind by")]
    public int BehindBy { get; set; } = source.BehindBy;

    [Display("Total commits")]
    public int TotalCommits { get; set; } = source.TotalCommits;

    [Display("HTML URL")]
    public string HtmlUrl { get; set; } = source.HtmlUrl ?? string.Empty;

    [Display("Diff URL")]
    public string DiffUrl { get; set; } = source.DiffUrl ?? string.Empty;

    [Display("Patch URL")]
    public string PatchUrl { get; set; } = source.PatchUrl ?? string.Empty;

    public IEnumerable<CompareCommitDto> Commits { get; set; } = (source.Commits ?? []).Select(x => new CompareCommitDto(x)).ToList();

    public IEnumerable<PullRequestFileDto> Files { get; set; } = (source.Files ?? []).Select(x => new PullRequestFileDto(x)).ToList();

    [Display("Files JSON")]
    public string FilesJson { get; set; } = JsonConvert.SerializeObject((source.Files ?? []).Select(x => new PullRequestFileDto(x)).ToList());
}
