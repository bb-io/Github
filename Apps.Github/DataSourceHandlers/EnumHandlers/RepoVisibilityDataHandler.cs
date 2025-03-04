using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.DataSourceHandlers.EnumHandlers;

public class RepoVisibilityDataHandler : IStaticDataSourceItemHandler
{
    private static Dictionary<string, string> EnumValues => new()
    {
        {"0", "Public"},
        {"1", "Private"},
        {"2", "Internal"},
    };

    public IEnumerable<DataSourceItem> GetData()
    {
        return EnumValues.Select(x => new DataSourceItem(x.Key, x.Value));
    }
}