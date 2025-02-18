using Apps.Github.Models.Commit.Requests;
using Apps.Github.Models.Respository.Requests;
using Apps.GitHub.Actions;
using Apps.GitHub.Models.Branch.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;
using Castle.Core.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Github.Base;

namespace Tests.Github
{
    [TestClass]
    public sealed class Test1 : TestBase
    {
        [TestInitialize]
        public void Init()
        {
            var outputDirectory = Path.Combine(GetTestFolderPath(), "Output");
            if (Directory.Exists(outputDirectory))
                Directory.Delete(outputDirectory, true);
            Directory.CreateDirectory(outputDirectory);
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            var actions = new FileActions(InvocationContext, FileManager);
            var repositoryRequest = new GetRepositoryRequest()
            { RepositoryId = "930409607" };
            var branchRequest = new GetOptionalBranchRequest();
            var fileRequest = new CreateOrUpdateFileRequest()
            { File = new Blackbird.Applications.Sdk.Common.Files.FileReference() { Name = "test.txt" } };

            await actions.CreateOrUpdateFile(repositoryRequest,branchRequest,fileRequest);


        }

        private string GetTestFolderPath()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            return config["TestFolder"];
        }
    }
}
