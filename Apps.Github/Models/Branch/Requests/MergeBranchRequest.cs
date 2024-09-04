using Apps.Github.DataSourceHandlers;
using Apps.GitHub.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.Models.Branch.Requests;

public class MergeBranchRequest
{
    [Display("Base branch")]
    [DataSource(typeof(BranchDataHandler))]
    public string BaseBranch { get; set; }

    [Display("Head branch")]
    [DataSource(typeof(BranchDataHandler))]
    public string HeadBranch { get; set; }

    [Display("Commit message")]
    public string CommitMessage { get; set; }
}