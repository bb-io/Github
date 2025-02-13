﻿using Octokit;

namespace Apps.GitHub.Dtos;

public class FileMetadataDto
{
    public FileMetadataDto(RepositoryContent content)
    {
        Name = content.Name;
        Path = content.Path;
    }

    public FileMetadataDto(TreeItem content)
    {
        Name = System.IO.Path.GetFileName(content.Path);
        Path = content.Path;
    }

    public string Name { get; set; }

    public string Path { get; set; }
}
