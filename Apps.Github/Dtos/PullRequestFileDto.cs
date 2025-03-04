using Blackbird.Applications.Sdk.Common;
using Octokit;

namespace Apps.GitHub.Dtos;

public class PullRequestFileDto(GitHubCommitFile gitHubCommitFile)
{
    public string Filename { get; set; } = gitHubCommitFile.Filename;
    
    [Display("Blob URL")] 
    public string BlobUrl { get; set; } = gitHubCommitFile.BlobUrl;

    [Display("Contents URL")]
    public string ContentsUrl { get; set; } = gitHubCommitFile.ContentsUrl;

    [Display("Raw URL")]
    public string RawUrl { get; set; } = gitHubCommitFile.RawUrl;

    [Display("ID")] 
    public string Id { get; set; } = gitHubCommitFile.Sha;

    public string Patch { get; set; } = gitHubCommitFile.Patch;
    
    [Display("Previous file name")] 
    public string PreviousFileName { get; set; } = gitHubCommitFile.PreviousFileName;
}