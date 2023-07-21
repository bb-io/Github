namespace Apps.Github.Models.Requests
{
    public class RepositoryContentRequest
    {
        public string RepositoryId { get; set; }

        public string? Path { get; set; }
    }
}
