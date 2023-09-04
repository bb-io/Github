using Blackbird.Applications.Sdk.Common;
using Octokit;

namespace Apps.Github.Dtos;

public class RepositoryDto
{
    public RepositoryDto(Repository source)
    {
        Id = source.Id.ToString();
        Name = source.Name;
        OwnerLogin = source.Owner.Login;
        CreatedAt = source.CreatedAt.DateTime;
    }

    public string Id { get; set; }

    public string Name { get; set; }
        
    [Display("Created at")]
    public DateTime CreatedAt { get; set; }

    public string OwnerLogin { get; set; }
}