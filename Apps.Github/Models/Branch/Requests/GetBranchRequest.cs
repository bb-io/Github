using Apps.Github.DataSourceHandlers;
using Apps.GitHub.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.Models.Branch.Requests;

public class GetBranchRequest
{
    [Display("Branch name")]
    [DataSource(typeof(BranchDataHandler))]
    public string Name { get; set; }
}