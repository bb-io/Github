using Apps.Github.Models.PullRequest.Payloads;
using Blackbird.Applications.Sdk.Common;
using Octokit;

namespace Apps.GitHub.Dtos;

public class PullRequestFileDto
{
    public string Filename { get; set; } = string.Empty;

    [Display("Blob URL")]
    public string BlobUrl { get; set; } = string.Empty;

    [Display("Contents URL")]
    public string ContentsUrl { get; set; } = string.Empty;

    [Display("Raw URL")]
    public string RawUrl { get; set; } = string.Empty;

    [Display("ID")]
    public string Id { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public int Additions { get; set; }

    public int Deletions { get; set; }

    public int Changes { get; set; }

    public string Patch { get; set; } = string.Empty;

    [Display("Previous file name")]
    public string PreviousFileName { get; set; } = string.Empty;

    public PullRequestFileDto()
    {
    }

    public PullRequestFileDto(PullRequestFile source)
    {
        Filename = source.FileName;
        BlobUrl = source.BlobUrl;
        ContentsUrl = source.ContentsUrl;
        RawUrl = source.RawUrl;
        Id = source.Sha;
        Status = source.Status;
        Additions = source.Additions;
        Deletions = source.Deletions;
        Changes = source.Changes;
        Patch = source.Patch ?? string.Empty;
        PreviousFileName = source.PreviousFileName ?? string.Empty;
    }

    public PullRequestFileDto(CompareFilePayload source)
    {
        Filename = source.Filename ?? string.Empty;
        BlobUrl = source.BlobUrl ?? string.Empty;
        ContentsUrl = source.ContentsUrl ?? string.Empty;
        RawUrl = source.RawUrl ?? string.Empty;
        Id = source.Sha ?? string.Empty;
        Status = source.Status ?? string.Empty;
        Additions = source.Additions;
        Deletions = source.Deletions;
        Changes = source.Changes;
        Patch = source.Patch ?? string.Empty;
        PreviousFileName = source.PreviousFilename ?? string.Empty;
    }
}
