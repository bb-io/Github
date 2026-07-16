using Apps.Github.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.Webhooks.Payloads;

public class PullRequestActionInput
{
    [Display("Action")]
    [StaticDataSource(typeof(PullRequestActionDataHandler))]
    public IEnumerable<string>? Action { get; set; }
}
