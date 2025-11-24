using Tests.Github.Base;
using Apps.GitHub.DataSourceHandlers;
using Apps.GitHub.Models.Branch.Requests;
using Apps.Github.Models.Respository.Requests;
using Blackbird.Applications.SDK.Extensions.FileManagement.Models.FileDataSourceItems;

namespace Tests.Github;

[TestClass]
public class FileDataHandlerTests : TestBase
{
    [TestMethod]
    public async Task GetFolderContentAsync_ReturnsFolderContent()
    {
        // Arrange
        var repoRequest = new GetRepositoryRequest { RepositoryId = "1027644029" };
        var branchRequest = new GetOptionalBranchRequest { };
        var handler = new FilePathDataHandler(InvocationContext, repoRequest, branchRequest);
        var folderContext = new FolderContentDataSourceContext { FolderId = "Shared/Filters/DogFilter.cs" };

        // Act
        var result = await handler.GetFolderContentAsync(folderContext, CancellationToken.None);

        // Assert
        foreach (var item in result)
            Console.WriteLine($"{item.Id} - {(item.Type == 0 ? "Folder" : "File")} - {item.DisplayName}");
        Assert.IsNotNull(result);
	}
}
