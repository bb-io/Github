using Octokit;

namespace Apps.Github.Dtos;

public class BranchDto
{
    public BranchDto(Branch source) 
    {
        Name = source.Name;
        Protected = source.Protected;
    }

    public string Name { get; set; }

    public bool Protected { get; set; }
}