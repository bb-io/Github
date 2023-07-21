namespace Apps.Github.Webhooks.Payloads
{
    public class PushPayload
    {
        public string Ref { get; set; }
        public string Before { get; set; }
        public string After { get; set; }
        public Repository Repository { get; set; }
        public Pusher Pusher { get; set; }
        public Sender Sender { get; set; }
        public bool Created { get; set; }
        public bool Deleted { get; set; }
        public bool Forced { get; set; }
        public string Compare { get; set; }
        public List<Commit> Commits { get; set; }
    }

    public class Author
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }

    public class Commit
    {
        public string Id { get; set; }
        public string TreeId { get; set; }
        public bool Distinct { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string Url { get; set; }
        public Author Author { get; set; }
        public Committer Committer { get; set; }
        public List<string> Added { get; set; }
        public List<string> Removed { get; set; }
        public List<string> Modified { get; set; }
    }

    public class Committer
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }

    public class Owner
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public int Id { get; set; }
        public string NodeId { get; set; }
        public string AvatarUrl { get; set; }
        public string GravatarId { get; set; }
        public string Url { get; set; }
        public string HtmlUrl { get; set; }
        public string Type { get; set; }
        public bool SiteAdmin { get; set; }
    }

    public class Pusher
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class Repository
    {
        public int Id { get; set; }
        public string NodeId { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public bool Private { get; set; }
        public Owner Owner { get; set; }
        public string HtmlUrl { get; set; }
        public string Description { get; set; }
        public bool Fork { get; set; }
        public string Url { get; set; }
        public int CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int PushedAt { get; set; }
        public string GitUrl { get; set; }
        public string SshUrl { get; set; }
        public string CloneUrl { get; set; }
        public string SvnUrl { get; set; }
        public int Size { get; set; }
        public int StargazersCount { get; set; }
        public int WatchersCount { get; set; }
        public bool HasIssues { get; set; }
        public bool HasProjects { get; set; }
        public bool HasDownloads { get; set; }
        public bool HasWiki { get; set; }
        public bool HasPages { get; set; }
        public bool HasDiscussions { get; set; }
        public int ForksCount { get; set; }
        public bool Archived { get; set; }
        public bool Disabled { get; set; }
        public int OpenIssuesCount { get; set; }
        public bool AllowForking { get; set; }
        public bool IsTemplate { get; set; }
        public bool WebCommitSignoffRequired { get; set; }
        public string Visibility { get; set; }
        public int Forks { get; set; }
        public int OpenIssues { get; set; }
        public int Watchers { get; set; }
        public string DefaultBranch { get; set; }
        public int Stargazers { get; set; }
        public string MasterBranch { get; set; }
    }

    public class Sender
    {
        public string Login { get; set; }
        public int Id { get; set; }
        public string NodeId { get; set; }
        public string AvatarUrl { get; set; }
        public string GravatarId { get; set; }
        public string Url { get; set; }
        public string HtmlUrl { get; set; }
        public string Type { get; set; }
        public bool SiteAdmin { get; set; }
    }
}
