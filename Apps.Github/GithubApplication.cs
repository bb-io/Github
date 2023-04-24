using Blackbird.Applications.Sdk.Common;

namespace Apps.Github
{
    public class GithubApplication : IApplication
    {
        public string Name
        {
            get => "Github";
            set { }
        }

        public T GetInstance<T>()
        {
            throw new NotImplementedException();
        }
    }
}
