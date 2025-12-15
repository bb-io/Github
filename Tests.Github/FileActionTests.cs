using Apps.Github.Models.Commit.Requests;
using Apps.Github.Models.Respository.Requests;
using Apps.GitHub.Actions;
using Apps.GitHub.Models.Branch.Requests;
using Apps.GitHub.Models.File.Requests;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Filters.Coders;
using Blackbird.Filters.Constants;
using Blackbird.Filters.Content;
using Blackbird.Filters.Enums;
using Blackbird.Filters.Transformations;
using Microsoft.Extensions.Configuration;
using Octokit;
using Tests.Github.Base;

namespace Tests.Github;

[TestClass]
public sealed class FileActionTests : TestBase
{
    [TestInitialize]
    public void Init()
    {
        var outputDirectory = Path.Combine(GetTestFolderPath(), "Output");
        if (Directory.Exists(outputDirectory))
        {
            Directory.Delete(outputDirectory, true);
        }
        
        Directory.CreateDirectory(outputDirectory);
    }

    [TestMethod]
    public async Task CreateOrUpdateFile_ValidInput_Success()
    {
        var actions = new FileActions(InvocationContext, FileManager);
        var repositoryRequest = new GetRepositoryRequest
        {
            RepositoryId = "930409607"
        };
        var branchRequest = new GetOptionalBranchRequest
        {
            Name = "main"
        };
        var fileRequest = new CreateOrUpdateFileRequest
        {
            File = new FileReference
            {
                Name = "test.txt"
            },
            CommitMessage = $"comes from test {DateTime.Now.TimeOfDay}", FolderPath = "folder"
        };
            
        try
        {
            await actions.CreateOrUpdateFile(repositoryRequest, branchRequest, fileRequest);
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }

        Assert.IsTrue(true);
    }

    private string GetTestFolderPath()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            
        return config["TestFolder"]!;
    }

    [TestMethod]
    public async Task Download_json_with_metadata()
    {
        var actions = new FileActions(InvocationContext, FileManager);
        var repositoryRequest = new GetRepositoryRequest
        {
            RepositoryId = "1116720412"
        };
        var branchRequest = new GetOptionalBranchRequest
        {
            
        };
        var fileRequest = new DownloadFileRequest
        {
            FilePath = "/lang/en.json"
        };


        var result = await actions.DownloadFile(repositoryRequest, branchRequest, fileRequest);

        var contentString = FileManager.ReadOutputAsString(result.Content);
        var codedContent = (new JsonContentCoder()).Deserialize(contentString, result.Content.Name);

        Assert.IsTrue(codedContent.Language == "en");
    }

    [TestMethod]
    public async Task Download_translate_upload_metadata()
    {
        var actions = new FileActions(InvocationContext, FileManager);
        var repositoryRequest = new GetRepositoryRequest
        {
            RepositoryId = "1116720412"
        };
        var branchRequest = new GetOptionalBranchRequest
        {

        };
        var fileRequest = new DownloadFileRequest
        {
            FilePath = "/lang/en.json"
        };


        var result = await actions.DownloadFile(repositoryRequest, branchRequest, fileRequest);

        var contentString = FileManager.ReadOutputAsString(result.Content);
        var transformation = Transformation.Parse(contentString, result.Content.Name);

        transformation.TargetLanguage = "pseudo";
        foreach (var unit in transformation.GetUnits())
        {
            foreach (var segment in unit.Segments.Where(x => !x.IsIgnorbale && x.IsInitial))
            {
                segment.SetTarget(segment.GetSource() + " - TRANSLATED");
                segment.State = SegmentState.Translated;
            }

            unit.AddUsage("Pseudo", 42, UsageUnit.Words);
            unit.Provenance.Translation.Tool = "Pseudo";
        }

        var translation = transformation.Serialize();

        var translatedFile = FileManager.CreateFileReferenceFromString(translation, MediaTypes.Xliff, transformation.XliffFileName);

        var uploadFileRequest = new CreateOrUpdateFileRequest
        {
            File = translatedFile,
            CommitMessage = $"From test {DateTime.Now.TimeOfDay}",
            FolderPath = "/lang",
            NewFileName = transformation.TargetLanguage
        };

        var uploadResponse = await actions.CreateOrUpdateFile(repositoryRequest, branchRequest, uploadFileRequest);

        var returnedContentString = FileManager.ReadOutputAsString(uploadResponse.Content);
        var returnedTransformation = Transformation.Parse(returnedContentString, uploadResponse.Content.Name);

        Assert.AreEqual("Github", returnedTransformation.TargetSystemReference.SystemName);
        Assert.AreEqual("pseudo", returnedTransformation.TargetLanguage);

    }
}