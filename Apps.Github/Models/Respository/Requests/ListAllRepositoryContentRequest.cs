using Apps.Github.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.Models.Respository.Requests
{
    public class ListAllRepositoryContentRequest
    {
        [Display("Repository")]
        [DataSource(typeof(RepositoryDataHandler))]
        public string RepositoryId { get; set;}
    }
}
