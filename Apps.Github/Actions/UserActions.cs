using Apps.Github.Actions.Base;
using Apps.Github.DataSourceHandlers;
using Apps.Github.Models.User.Requests;
using Apps.Github.Models.User.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Github.Actions;

[ActionList]
public class UserActions : GithubActions
{
    public UserActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("Get user by username", Description = "Get information about specific user")]
    public async Task<UserDataResponse> GetUserData([ActionParameter] UserDataRequest input)
    {
        var user = await ClientSdk.User.Get(input.Username);
        return new(user);
    }

    [Action("Get user", Description = "Get information about specific user")]
    public async Task<UserDataResponse> GetUser(
        [ActionParameter] [Display("Username")] [DataSource(typeof(UsersDataHandler))] string username)
    {
        var user = await ClientSdk.User.Get(username);
        return new(user);
    }

    [Action("Get my user data", Description = "Get my user data")]
    public async Task<UserDataResponse> GetMyUser()
    {
        var user = await ClientSdk.User.Current();
        return new(user);
    }
}