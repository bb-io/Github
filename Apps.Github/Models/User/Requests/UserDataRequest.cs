using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.User.Requests
{
    public class UserDataRequest
    {
        [Display("User login")]
        public string UserLogin { get; set; }
    }
}
