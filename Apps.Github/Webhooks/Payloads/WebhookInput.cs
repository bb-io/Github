using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Webhooks.Payloads
{
    public class WebhookInput
    {
        [Display("Repository ID")]
        public string RepositoryId { get; set; }
    }
}
