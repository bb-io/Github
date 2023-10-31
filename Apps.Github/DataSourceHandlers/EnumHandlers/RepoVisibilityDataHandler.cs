using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Github.DataSourceHandlers.EnumHandlers
{
    public class RepoVisibilityDataHandler : EnumDataHandler
    {
        protected override Dictionary<string, string> EnumValues => new()
        {
            {"0", "Public"},
            {"1", "Private"},
            {"2", "Internal"},
        };
    }
}
