using Blackbird.Applications.Sdk.Common;
using Octokit;

namespace Apps.Github.Dtos;

public class CommitFileDto(GitHubCommitFile gitHubCommitFile)
{
    [Display("File name")]
    public string Filename { get; set; } = gitHubCommitFile.Filename;
    
    public int Additions { get; set; } = gitHubCommitFile.Additions;
    
    public int Deletions { get; set; } = gitHubCommitFile.Deletions;
    
    public int Changes { get; set; } = gitHubCommitFile.Changes;
    
    public string Status { get; set; } = gitHubCommitFile.Status;
    
    [Display("Blob URL")] 
    public string BlobUrl { get; set; } = gitHubCommitFile.BlobUrl;

    [Display("Contents URL")] 
    public string ContentsUrl { get; set; } = gitHubCommitFile.ContentsUrl;

    [Display("Raw URL")] 
    public string RawUrl { get; set; } = gitHubCommitFile.RawUrl;

    [Display("Commit file ID")] 
    public string Id { get; set; } = gitHubCommitFile.Sha;
    
    public string Patch { get; set; } = gitHubCommitFile.Patch;
    
    [Display("Previous file name")]
    public string PreviousFileName { get; set; } = gitHubCommitFile.PreviousFileName;
}