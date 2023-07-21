namespace Apps.Github.Webhooks.Payloads
{
    public class PullRequestPayload
    {
        public string Action { get; set; }
        public int Number { get; set; }
        public PullRequest Pull_request { get; set; }
        public Repository Repository { get; set; }
        public Sender Sender { get; set; }
        public Installation Installation { get; set; }
    }

    public class Base
    {
        public string Label { get; set; }
        public string Ref { get; set; }
        public string Sha { get; set; }
        public User User { get; set; }
        public Repo Repo { get; set; }
    }

    public class Comments
    {
        public string Href { get; set; }
    }

    public class Commits
    {
        public string Href { get; set; }
    }

    public class Head
    {
        public string Label { get; set; }
        public string Ref { get; set; }
        public string Sha { get; set; }
        public User User { get; set; }
        public Repo Repo { get; set; }
    }

    public class Html
    {
        public string Href { get; set; }
    }

    public class Installation
    {
        public int Id { get; set; }
        public string NodeId { get; set; }
    }

    public class Issue
    {
        public string Href { get; set; }
    }

    public class Links
    {
        public Self Self { get; set; }
        public Html Html { get; set; }
        public Issue Issue { get; set; }
        public Comments Comments { get; set; }
        public ReviewComments ReviewComments { get; set; }
        public ReviewComment ReviewComment { get; set; }
        public Commits Commits { get; set; }
        public Statuses Statuses { get; set; }
    }

    public class PullRequest
    {
        public string Url { get; set; }
        public int Id { get; set; }
        public string NodeId { get; set; }
        public string HtmlUrl { get; set; }
        public string DiffUrl { get; set; }
        public string PatchUrl { get; set; }
        public string IssueUrl { get; set; }
        public int Number { get; set; }
        public string State { get; set; }
        public bool Locked { get; set; }
        public string Title { get; set; }
        public User User { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ClosedAt { get; set; }
        public string MergedAt { get; set; }
        public bool Draft { get; set; }
        public string CommitsUrl { get; set; }
        public string ReviewCommentsUrl { get; set; }
        public string ReviewCommentUrl { get; set; }
        public string CommentsUrl { get; set; }
        public string StatusesUrl { get; set; }
        public Head Head { get; set; }
        public Base Base { get; set; }
        public Links Links { get; set; }
        public string AuthorAssociation { get; set; }
        public bool Merged { get; set; }
        public string MergeableState { get; set; }
        public int Comments { get; set; }
        public int ReviewComments { get; set; }
        public bool MaintainerCanModify { get; set; }
        public int Commits { get; set; }
        public int Additions { get; set; }
        public int Deletions { get; set; }
        public int ChangedFiles { get; set; }
    }

    public class Repo
    {
        public int Id { get; set; }
        public string NodeId { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public bool Private { get; set; }
        public Owner Owner { get; set; }
        public string HtmlUrl { get; set; }
        public object Description { get; set; }
        public bool Fork { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime PushedAt { get; set; }
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
        public List<object> Topics { get; set; }
        public string Visibility { get; set; }
        public int Forks { get; set; }
        public int OpenIssues { get; set; }
        public int Watchers { get; set; }
        public string DefaultBranch { get; set; }
        public bool AllowSquashMerge { get; set; }
        public bool AllowMergeCommit { get; set; }
        public bool AllowRebaseMerge { get; set; }
        public bool AllowAutoMerge { get; set; }
        public bool DeleteBranchOnMerge { get; set; }
        public bool AllowUpdateBranch { get; set; }
        public bool UseSquashPrTitleAsDefault { get; set; }
        public string SquashMergeCommitMessage { get; set; }
        public string SquashMergeCommitTitle { get; set; }
        public string MergeCommitMessage { get; set; }
        public string MergeCommitTitle { get; set; }
    }

    public class ReviewComment
    {
        public string Href { get; set; }
    }

    public class ReviewComments
    {
        public string Href { get; set; }
    }

    public class Self
    {
        public string Href { get; set; }
    }

    public class Statuses
    {
        public string Href { get; set; }
    }

    public class User
    {
        public string Login { get; set; }
        public int Id { get; set; }
        public string NodeId { get; set; }
        public string AvatarUrl { get; set; }
        public string GravatarId { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public bool SiteAdmin { get; set; }
    }
}
