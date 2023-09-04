using Apps.Github.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.Models.Commit.Requests;

public class ListRepositoryCommitsRequest
{
    [Display("Repository")]
    [DataSource(typeof(RepositoryDataHandler))]
    public string RepositoryId { get; set; }
}