using Apps.Github.Models.User.Requests;
using Apps.Github.Models.User.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;

namespace Apps.Github.Actions
{
    [ActionList]
    public class UserActions
    {
        [Action("Get user data", Description = "Get information about specific user")]
        public UserDataResponse GetUserData(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] UserDataRequest input)
        {
            var githubClient = new BlackbirdGithubClient(authenticationCredentialsProviders);
            var user = githubClient.User.Get(input.UserLogin).Result;
            return new UserDataResponse()
            {
                Name = user.Name,
                UserUrl = user.Url,
                PublicRepositoriesNumber = user.PublicRepos
            };
        }
    }
}
