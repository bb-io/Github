using Newtonsoft.Json;

namespace Apps.Github.Models.PullRequest.Payloads;

public class CompareCommitsPayload
{
    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("ahead_by")]
    public int AheadBy { get; set; }

    [JsonProperty("behind_by")]
    public int BehindBy { get; set; }

    [JsonProperty("total_commits")]
    public int TotalCommits { get; set; }

    [JsonProperty("html_url")]
    public string? HtmlUrl { get; set; }

    [JsonProperty("diff_url")]
    public string? DiffUrl { get; set; }

    [JsonProperty("patch_url")]
    public string? PatchUrl { get; set; }

    [JsonProperty("commits")]
    public List<CompareCommitPayload>? Commits { get; set; }

    [JsonProperty("files")]
    public List<CompareFilePayload>? Files { get; set; }
}

public class CompareCommitPayload
{
    [JsonProperty("sha")]
    public string? Sha { get; set; }

    [JsonProperty("url")]
    public string? Url { get; set; }

    [JsonProperty("commit")]
    public CompareCommitDetailsPayload? Commit { get; set; }

    [JsonProperty("author")]
    public GithubUserPayload? Author { get; set; }
}

public class CompareCommitDetailsPayload
{
    [JsonProperty("message")]
    public string? Message { get; set; }
}

public class CompareFilePayload
{
    [JsonProperty("sha")]
    public string? Sha { get; set; }

    [JsonProperty("filename")]
    public string? Filename { get; set; }

    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("additions")]
    public int Additions { get; set; }

    [JsonProperty("deletions")]
    public int Deletions { get; set; }

    [JsonProperty("changes")]
    public int Changes { get; set; }

    [JsonProperty("blob_url")]
    public string? BlobUrl { get; set; }

    [JsonProperty("raw_url")]
    public string? RawUrl { get; set; }

    [JsonProperty("contents_url")]
    public string? ContentsUrl { get; set; }

    [JsonProperty("patch")]
    public string? Patch { get; set; }

    [JsonProperty("previous_filename")]
    public string? PreviousFilename { get; set; }
}
