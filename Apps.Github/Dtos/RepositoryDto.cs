using Octokit;

namespace Apps.Github.Dtos
{
    public class RepositoryDto
    {
        public RepositoryDto(Repository source)
        {
            Id = source.Id.ToString();
            Name = source.Name;
            OwnerLogin = source.Owner.Login;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string OwnerLogin { get; set; }
    }
}
