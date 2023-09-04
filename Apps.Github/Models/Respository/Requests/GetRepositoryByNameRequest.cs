using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.Respository.Requests;

public class GetRepositoryByNameRequest
{
    [Display("Repository owner")]
    public string RepositoryOwner { get; set; }

    [Display("Repository name")]
    public string RepositoryName { get; set; }
}