using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.Respository.Responses;

public class RepositoryContentPathsResponse
{
    public IEnumerable<RepositoryItem> Items { get; set; }  
}

public class RepositoryItem
{
    public string Path { get; set; }

    public string Sha { get; set; }
    
    [Display("Is folder")]
    public bool IsFolder { get; set; }
}