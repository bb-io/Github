using Blackbird.Applications.Sdk.Common;
using Octokit;

namespace Apps.Github.Dtos;

public class RepositoryDto(Repository source)
{
    [Display("ID")]
    public string Id { get; set; } = source.Id.ToString();

    public string Name { get; set; } = source.Name;

    [Display("Created at")]
    public DateTime CreatedAt { get; set; } = source.CreatedAt.DateTime;

    [Display("Owner logged in?")]
    public string OwnerLogin { get; set; } = source.Owner.Login;
}