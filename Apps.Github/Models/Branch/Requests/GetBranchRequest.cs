using Apps.Github.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.Models.Branch.Requests
{
    public class GetBranchRequest
    {
        [Display("Repository")]
        [DataSource(typeof(RepositoryDataHandler))]
        public string RepositoryId { get; set;}
        public string Name { get; set; }
    }
}
