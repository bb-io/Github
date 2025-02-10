using Apps.GitHub.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.Models.PullRequest.Requests;

public class CreatePullRequest
{    
    public string Title { get; set; }

    public string? Description { get; set; }

    [Display("Head branch")]
    [DataSource(typeof(BranchDataHandler))]
    public string HeadBranch { get; set; }

    [Display("Base branch")]
    [DataSource(typeof(BranchDataHandler))]
    public string BaseBranch { get; set; }
}