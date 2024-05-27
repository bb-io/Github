using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.GitHub.Models.Commit.Requests
{
    public class UpdateFilesInZipRequest
    {
        [Display("ZIP File")]
        public FileReference File { get; set; }

        [Display("Commit message")]
        public string CommitMessage { get; set; }
    }
}
