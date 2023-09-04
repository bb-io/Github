using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Webhooks.Payloads;

public class FolderInput
{
    [Display("Folder path")]
    public string? FolderPath { get; set; }
}