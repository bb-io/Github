namespace Apps.Github.Webhooks.Payloads;

public class PullRequestPayloadFlat
{
    public PullRequestPayloadFlat(PullRequestPayload source) { 
        Action = source.Action;
        Number = source.Number;
        PullRequestUrl = source.Pull_request.Url;
        PullRequestId = source.Pull_request.Id;
        PullRequestHtmlUrl = source.Pull_request.HtmlUrl;
        PullRequestTitle = source.Pull_request.Title;
        PullRequestBody = source.Pull_request.Body ?? string.Empty;
        HeadSha = source.Pull_request.Head.Sha;
        HeadRef = source.Pull_request.Head.Ref;
        BaseSha = source.Pull_request.Base.Sha;
        BaseRef = source.Pull_request.Base.Ref;
        Draft = source.Pull_request.Draft;
        RepositoryId = source.Repository.Id.ToString();
        RepositoryName = source.Repository.Name;
        RepositoryFullName = string.IsNullOrWhiteSpace(source.Repository.FullName)
            ? $"{source.Repository.Owner?.Login}/{source.Repository.Name}".TrimStart('/')
            : source.Repository.FullName;
        SenderLogin = source.Sender.Login;
        SenderId = source.Sender.Id;
    }

    public PullRequestPayloadFlat()
    {
    }

    public string Action { get; set; }
    public int Number { get; set; }
    public string PullRequestUrl { get; set; }
    public string PullRequestHtmlUrl { get; set; }
    public int PullRequestId { get; set; }
    public string PullRequestTitle { get; set; }
    public string PullRequestBody { get; set; }
    public string HeadSha { get; set; }
    public string HeadRef { get; set; }
    public string BaseSha { get; set; }
    public string BaseRef { get; set; }
    public bool Draft { get; set; }
    public string RepositoryId { get; set; }
    public string RepositoryName { get; set; }
    public string RepositoryFullName { get; set; }
    public string SenderLogin { get; set; }
    public int SenderId { get; set; }
}
