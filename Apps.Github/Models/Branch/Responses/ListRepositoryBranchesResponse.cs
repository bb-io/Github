using Apps.Github.Dtos;

namespace Apps.Github.Models.Branch.Responses;

public class ListRepositoryBranchesResponse
{
    public IEnumerable<BranchDto> Branches { get; set; }
}