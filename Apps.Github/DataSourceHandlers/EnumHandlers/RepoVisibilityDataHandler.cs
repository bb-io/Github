using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
