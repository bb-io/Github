using Apps.Github.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.Models.Commit.Requests
{
    public class DeleteFileRequest
    {
        [Display("Repository")]
        [DataSource(typeof(RepositoryDataHandler))]
        public string RepositoryId { get; set; }

        [Display("File path")]
        public string FilePath { get; set; }

        [Display("Commit message")]
        public string CommitMessage { get; set; }
    }
}
