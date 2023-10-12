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
    public UserDataResponse GetUserData(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] UserDataRequest input)
    {
        var githubClient = new BlackbirdGithubClient(authenticationCredentialsProviders);
        var user = githubClient.User.Get(input.Username).Result;
        return new UserDataResponse(user);
    }

    [Action("Get user", Description = "Get information about specific user")]
    public UserDataResponse GetUser(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] [Display("Username")] [DataSource(typeof(UsersDataHandler))] string username)
    {
        var githubClient = new BlackbirdGithubClient(authenticationCredentialsProviders);
        var user = githubClient.User.Get(username).Result;
        return new UserDataResponse(user);
    }
}