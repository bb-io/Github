using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.PullRequest.Responses
{
    public class IsPullMergedResponse
    {
        [Display("Is merged")]
        public bool IsPullMerged { get; set; }
    }
}
