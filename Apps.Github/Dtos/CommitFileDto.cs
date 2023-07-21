using Blackbird.Applications.Sdk.Common;
using Octokit;

namespace Apps.Github.Dtos;

public class CommitFileDto
{
    public CommitFileDto(GitHubCommitFile gitHubCommitFile)
    {
        Filename = gitHubCommitFile.Filename;
        Additions = gitHubCommitFile.Additions;
        Deletions = gitHubCommitFile.Deletions;
        Changes = gitHubCommitFile.Changes;
        Status = gitHubCommitFile.Status;
        BlobUrl = gitHubCommitFile.BlobUrl;
        ContentsUrl = gitHubCommitFile.ContentsUrl;
        RawUrl = gitHubCommitFile.RawUrl;
        Id = gitHubCommitFile.Sha;
        Patch = gitHubCommitFile.Patch;
        PreviousFileName = gitHubCommitFile.PreviousFileName;
    }

    public string Filename { get; set; }
    public int Additions { get; set; }
    public int Deletions { get; set; }
    public int Changes { get; set; }
    public string Status { get; set; }
    [Display("Blob URL")] public string BlobUrl { get; set; }

    [Display("Contents URL")] public string ContentsUrl { get; set; }

    [Display("Raw URL")] public string RawUrl { get; set; }

    [Display("ID")] public string Id { get; set; }
    public string Patch { get; set; }
    [Display("Previous file name")] public string PreviousFileName { get; set; }
}