using Apps.Github.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.Github.Models.Commit.Requests
{
    public class PushFileRequest
    {
        [Display("Repository")]
        [DataSource(typeof(RepositoryDataHandler))]
        public string RepositoryId { get; set; }

        [Display("Destination file path (e.g. \"Test/testFile.txt\")")]
        public string DestinationFilePath { get; set; }

        [Display("File")]
        public File File { get; set; }

        [Display("Commit message")]
        public string CommitMessage { get; set; }
    }
}
