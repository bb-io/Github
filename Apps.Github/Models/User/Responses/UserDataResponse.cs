using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.User.Responses;

public class UserDataResponse
{
    public string Name { get; set; }

    [Display("User url")]
    public string UserUrl { get; set; }

    [Display("Number of public repositories")]
    public int PublicRepositoriesNumber { get; set; }
}