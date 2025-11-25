using Tests.Github.Base;
using Apps.Github.DataSourceHandlers;
using Apps.GitHub.DataSourceHandlers;
using Apps.GitHub.Models.Branch.Requests;
using Apps.Github.Models.Respository.Requests;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.SDK.Extensions.FileManagement.Models.FileDataSourceItems;

namespace Tests.Github;

[TestClass]
public class DataHandlerTests : TestBase
{
    [TestMethod]
    public async Task RepositoryDataHandler_IsSuccess()
    {
        var handler = new RepositoryDataHandler(InvocationContext);

        var repositories = await handler.GetDataAsync(new DataSourceContext { SearchString=""}, CancellationToken.None);

        PrintResult(repositories);
        Assert.IsNotNull(repositories);
    }

    [TestMethod]
    public async Task WorkflowDataHandler_IsSuccess()
    {
        var handler = new WorkflowDataHandler(InvocationContext, new GetRepositoryRequest { RepositoryId= "1077915437" });

        var repositories = await handler.GetDataAsync(new DataSourceContext { SearchString = "" }, CancellationToken.None);

        PrintResult(repositories);
        Assert.IsNotNull(repositories);
    }

    [TestMethod]
    public async Task FilePathDataHandler_ReturnsFolderContent()
    {
        // Arrange
        var repoRequest = new GetRepositoryRequest { RepositoryId = "1101419096" };
        var branchRequest = new GetOptionalBranchRequest { Name = "develop" };
        var handler = new FilePathDataHandler(InvocationContext, repoRequest, branchRequest);
        var folderContext = new FolderContentDataSourceContext { FolderId = "Apps.PropioOne" };

        // Act
        var result = await handler.GetFolderContentAsync(folderContext, CancellationToken.None);

        // Assert
        foreach (var item in result)
            Console.WriteLine($"{item.Id} - {(item.Type == 0 ? "Folder" : "File")} - {item.DisplayName}");
        Assert.IsNotNull(result);
    }

    private static void PrintResult(IEnumerable<DataSourceItem> items)
    {
        foreach (var item in items)
            Console.WriteLine($"{item.Value} - {item.DisplayName}");
    }
}
