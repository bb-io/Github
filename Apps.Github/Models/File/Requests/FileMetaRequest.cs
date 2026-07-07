using Blackbird.Applications.Sdk.Common;

namespace Apps.GitHub.Models.File.Requests;
public class FileMetaRequest
{
    [Display("Source language code", Description = "The language of the file used in later Actions.")]
    public string? LanguageCode { get; set; }

    [Display("Content ID", Description = "The ID of the content, used by Blacklake when diffing.")]
    public string? ContentId { get; set; }
}
