using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.PullRequest.Requests;

public class CompareCommitsRequest
{
    [Display("Base reference", Description = "Usually the last reviewed commit SHA")]
    public string BaseReference { get; set; } = string.Empty;

    [Display("Head reference", Description = "Usually the current pull request head SHA")]
    public string HeadReference { get; set; } = string.Empty;
}
