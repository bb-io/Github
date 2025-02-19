using Apps.Github.Models.Commit.Requests;
using Apps.Github.Models.Respository.Requests;
using Apps.GitHub.Actions;
using Apps.GitHub.Models.Branch.Requests;
using Microsoft.Extensions.Configuration;
using Tests.Github.Base;

namespace Tests.Github
{
    [TestClass]
    public sealed class FileActionTests : TestBase
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
        public async Task CreateOrUpdateFile_ValidInput_Success()
        {
            var actions = new FileActions(InvocationContext, FileManager);
            var repositoryRequest = new GetRepositoryRequest()
            { RepositoryId = "930409607" };
            var branchRequest = new GetOptionalBranchRequest()
            { Name = "main" };
            var fileRequest = new CreateOrUpdateFileRequest()
            { File = new Blackbird.Applications.Sdk.Common.Files.FileReference() { Name = "test.txt" }, CommitMessage = $"comes from test {DateTime.Now.TimeOfDay}", FilePath = "folder" };
            try
            {
                await actions.CreateOrUpdateFile(repositoryRequest, branchRequest, fileRequest);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
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
