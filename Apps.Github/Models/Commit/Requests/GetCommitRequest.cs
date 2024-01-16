using Apps.Github.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.Models.Commit.Requests;

public class GetCommitRequest
{
    [Display("Repository ID")]
    [DataSource(typeof(RepositoryDataHandler))]
    public string RepositoryId { get; set; }

    [Display("Commit ID")]
    public string CommitId { get; set; }
}