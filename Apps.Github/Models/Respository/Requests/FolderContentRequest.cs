﻿using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.Respository.Requests;

public class FolderContentRequest
{
    public FolderContentRequest()
    {
    }

    public FolderContentRequest(string? path, bool? includeSubfolders)
    {
        Path = path;
        IncludeSubfolders = includeSubfolders;
    }

    [Display("Folder path (e.g. \"Folder1/Folder2\")")]
    public string? Path { get; set; }

    [Display("Include subfolders")]
    public bool? IncludeSubfolders { get; set; }

    [Display("Filter", Description = "Use the forward slash '/' to represent directory separator. Use '*' to represent wildcards in file and directory names. Use '**' to represent arbitrary directory depth.")]
    public string? Filter { get; set; }
}