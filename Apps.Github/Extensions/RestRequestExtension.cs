using Apps.GitHub.Models.Branch.Requests;
using RestSharp;

namespace Apps.GitHub.Extensions;

public static class RestRequestExtension
{
    public static RestRequest AddGithubBranch(this RestRequest request, GetOptionalBranchRequest branch)
    {
        if(!string.IsNullOrEmpty(branch?.Name))
            request.AddQueryParameter("ref", branch.Name);
        return request;
    }
}
