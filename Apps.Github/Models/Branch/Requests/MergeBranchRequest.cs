using Apps.Github.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.Models.Branch.Requests;

public class MergeBranchRequest
{
    [Display("Repository ID")]
    [DataSource(typeof(RepositoryDataHandler))]
    public string RepositoryId { get; set; }

    [Display("Base branch")]
    public string BaseBranch { get; set; }

    [Display("Head branch")]
    public string HeadBranch { get; set; }

    [Display("Commit message")]
    public string CommitMessage { get; set; }
}