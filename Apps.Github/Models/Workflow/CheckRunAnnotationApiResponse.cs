using Newtonsoft.Json;

namespace Apps.GitHub.Models.Workflow;

public class CheckRunAnnotationApiResponse
{
    [JsonProperty("path")]
    public string? Path { get; set; }

    [JsonProperty("start_line")]
    public int StartLine { get; set; }

    [JsonProperty("end_line")]
    public int EndLine { get; set; }

    [JsonProperty("start_column")]
    public int? StartColumn { get; set; }

    [JsonProperty("end_column")]
    public int? EndColumn { get; set; }

    [JsonProperty("annotation_level")]
    public string? AnnotationLevel { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("message")]
    public string? Message { get; set; }

    [JsonProperty("raw_details")]
    public string? RawDetails { get; set; }

    [JsonProperty("blob_href")]
    public string? BlobHref { get; set; }
}
