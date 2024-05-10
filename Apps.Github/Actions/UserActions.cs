using Apps.Github.DataSourceHandlers;
using Apps.Github.Models.User.Requests;
using Apps.Github.Models.User.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.Actions;

[ActionList]
public class UserActions
{
    [Action("Get user by username", Description = "Get information about specific user")]
    public async Task<UserDataResponse> GetUserData(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] UserDataRequest input)
    {
        var githubClient = new BlackbirdGithubClient(authenticationCredentialsProviders);
        var user = await githubClient.User.Get(input.Username);
        return new(user);
    }

    [Action("Get user", Description = "Get information about specific user")]
    public async Task<UserDataResponse> GetUser(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] [Display("Username")] [DataSource(typeof(UsersDataHandler))] string username)
    {
        var githubClient = new BlackbirdGithubClient(authenticationCredentialsProviders);
        var user = await githubClient.User.Get(username);
        return new(user);
    }

    [Action("Get my user data", Description = "Get my user data")]
    public async Task<UserDataResponse> GetMyUser(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
    {
        var githubClient = new BlackbirdGithubClient(authenticationCredentialsProviders);
        var user = await githubClient.User.Current();
        return new(user);
    }
}