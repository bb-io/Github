using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Webhooks.Payloads;

public class FolderInput
{
    [Display("Folder path", Description = "Use the forward slash '/' to represent directory separator. Use '*' to represent wildcards in file and directory names. Use '**' to represent arbitrary directory depth.")]
    public string? FolderPath { get; set; }
}