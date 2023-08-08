using Apps.Github.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.Models.PullRequest.Requests
{
    public class GetPullRequest
    {
        [Display("Repository")]
        [DataSource(typeof(RepositoryDataHandler))]
        public string RepositoryId { get; set; }

        [Display("Pull request number")]
        public string PullRequestNumber { get; set; }
    }
}
