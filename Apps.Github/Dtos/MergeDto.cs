using Octokit;

namespace Apps.Github.Dtos;

public class MergeDto(Merge source)
{
    public string Id { get; set; } = source.Ref;
}