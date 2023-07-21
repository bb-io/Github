namespace Apps.Github.Models.Requests
{
    public class GetFileRequest
    {
        public string RepositoryId { get; set; }
        public string FilePath { get; set; }
    }
}
