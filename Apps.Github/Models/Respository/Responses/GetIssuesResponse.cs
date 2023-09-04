using Apps.Github.Dtos;

namespace Apps.Github.Models.Respository.Responses;

public class GetIssuesResponse
{
    public IEnumerable<IssueDto> Issues { get; set; }
}