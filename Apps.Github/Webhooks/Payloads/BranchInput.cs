using Apps.GitHub.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.GitHub.Webhooks.Payloads
{
    public class BranchInput
    {
        [Display("Branch name")]
        [DataSource(typeof(BranchDataHandler))]
        public string? BranchName { get; set; }
    }
}
