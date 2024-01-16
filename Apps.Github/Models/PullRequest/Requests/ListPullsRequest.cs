using Apps.Github.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.Models.PullRequest.Requests;

public class ListPullsRequest
{
    [Display("Repository ID")]
    [DataSource(typeof(RepositoryDataHandler))]
    public string RepositoryId { get; set; }
}