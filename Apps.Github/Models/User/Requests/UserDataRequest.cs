using Apps.Github.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.Models.User.Requests;

public class UserDataRequest
{
    [Display("Username")]
    public string Username { get; set; }
}