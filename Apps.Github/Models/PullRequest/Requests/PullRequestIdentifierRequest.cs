using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.PullRequest.Requests;

public class PullRequestIdentifierRequest
{
    [Display("Pull request number")]
    public string PullRequestNumber { get; set; } = string.Empty;
}
