using Apps.Github.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.Models.PullRequest.Requests;

public class CreatePullRequest
{
    [Display("Repository")]
    [DataSource(typeof(RepositoryDataHandler))]
    public string RepositoryId { get; set; }

    public string Title { get; set; }

    [Display("Head branch")]
    public string HeadBranch { get; set; }

    [Display("Base branch")]
    public string BaseBranch { get; set; }
}