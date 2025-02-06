using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.User.Responses;

public class UserDataResponse
{
    public UserDataResponse(Octokit.User input)
    {
        Name = input.Name;
        AvatarUrl = input.AvatarUrl;
        Bio = input.Bio;
        Blog = input.Blog;
        Collaborators = input.Collaborators;
        Company = input.Company;
        CreatedAt = input.CreatedAt.DateTime;
        DiskUsage = input.DiskUsage;
        Email = input.Email;
        Followers = input.Followers;
        Following = input.Following;
        Hireable = input.Hireable;
        HtmlUrl = input.HtmlUrl;
        Id = Convert.ToInt32(input.Id);
        NodeId = input.NodeId;
        Location = input.Location;
        Login = input.Login;
        OwnedPrivateRepos = input.OwnedPrivateRepos;
        PrivateGists = input.PrivateGists;
        PublicRepos = input.PublicRepos;
        TotalPrivateRepos = input.TotalPrivateRepos;
        Url = input.Url;
    }
    public string Name { get; set; }

    [Display("Avatar url")]
    public string AvatarUrl { get; set; }

    [Display("Account's bio")]
    public string Bio { get; set; }

    public string Blog { get; set; }

    public int? Collaborators { get; set; }

    public string Company { get; set; }

    [Display("Created at")]
    public DateTime CreatedAt { get; set; }

    [Display("Disk usage")]
    public int? DiskUsage { get; set; }

    public string Email { get; set; }

    public int Followers { get; set; }

    public int Following { get; set; }

    public bool? Hireable { get; set; }

    [Display("HTML url")]
    public string HtmlUrl { get; set; }
    public int Id { get; set; }

    [Display("GraphQL Node Id")]
    public string NodeId { get; set; }

    public string Location { get; set; }

    public string Login { get; set; }

    [Display("Owned private repositories")]
    public int OwnedPrivateRepos { get; set; }

    [Display("Private gists")]
    public int? PrivateGists { get; set; }

    [Display("Public gists")]
    public int PublicGists { get; set; }

    [Display("Public respos")]
    public int PublicRepos { get; set; }

    [Display("Total private repos")]
    public int TotalPrivateRepos { get; set; }

    [Display("The account's API URL")]
    public string Url { get; set; }
}