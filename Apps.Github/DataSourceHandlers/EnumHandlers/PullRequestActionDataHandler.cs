using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.DataSourceHandlers.EnumHandlers;

public class PullRequestActionDataHandler : IStaticDataSourceItemHandler
{
    private static readonly Dictionary<string, string> ActionValues = new()
    {
        { "assigned", "Assigned" },
        { "auto_merge_disabled", "Auto merge disabled" },
        { "auto_merge_enabled", "Auto merge enabled" },
        { "closed", "Closed" },
        { "converted_to_draft", "Converted to draft" },
        { "demilestoned", "Demilestoned" },
        { "dequeued", "Dequeued" },
        { "edited", "Edited" },
        { "enqueued", "Enqueued" },
        { "labeled", "Labeled" },
        { "locked", "Locked" },
        { "milestoned", "Milestoned" },
        { "opened", "Opened" },
        { "ready_for_review", "Ready for review" },
        { "reopened", "Reopened" },
        { "review_request_removed", "Review request removed" },
        { "review_requested", "Review requested" },
        { "synchronize", "Synchronize" },
        { "unassigned", "Unassigned" },
        { "unlabeled", "Unlabeled" },
        { "unlocked", "Unlocked" }
    };

    public IEnumerable<DataSourceItem> GetData()
    {
        return ActionValues.Select(x => new DataSourceItem(x.Key, x.Value));
    }
}
