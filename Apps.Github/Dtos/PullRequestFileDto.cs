using Blackbird.Applications.Sdk.Common;
using Octokit;

namespace Apps.GitHub.Dtos
{
    public class PullRequestFileDto
    {
        public PullRequestFileDto(GitHubCommitFile gitHubCommitFile)
        {
            Filename = gitHubCommitFile.Filename;
            BlobUrl = gitHubCommitFile.BlobUrl;
            ContentsUrl = gitHubCommitFile.ContentsUrl;
            RawUrl = gitHubCommitFile.RawUrl;
            Id = gitHubCommitFile.Sha;
            PreviousFileName = gitHubCommitFile.PreviousFileName;
        }

        public string Filename { get; set; }
        [Display("Blob URL")] public string BlobUrl { get; set; }

        [Display("Contents URL")] public string ContentsUrl { get; set; }

        [Display("Raw URL")] public string RawUrl { get; set; }

        [Display("ID")] public string Id { get; set; }
        public string Patch { get; set; }
        [Display("Previous file name")] public string PreviousFileName { get; set; }
    }
}
