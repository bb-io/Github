using Apps.Github.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.Models.Commit.Requests
{
    public class PushFileRequest
    {
        [Display("Repository")]
        [DataSource(typeof(RepositoryDataHandler))]
        public string RepositoryId { get; set; }

        [Display("Destination file path (e.g. \"Test/testFile.txt\")")]
        public string DestinationFilePath { get; set; }

        public byte[] File { get; set; }

        [Display("Commit message")]
        public string CommitMessage { get; set; }
    }
}
