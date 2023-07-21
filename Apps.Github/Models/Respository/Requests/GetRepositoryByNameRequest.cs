namespace Apps.Github.Models.Respository.Requests
{
    public class GetRepositoryByNameRequest
    {
        public string RepositoryOwner { get; set; }
        public string RepositoryName { get; set; }
    }
}
