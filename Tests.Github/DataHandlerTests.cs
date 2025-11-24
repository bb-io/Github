using Tests.Github.Base;
using Apps.Github.DataSourceHandlers;
using Apps.GitHub.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common.Dynamic;

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
        var handler = new WorkflowDataHandler(InvocationContext, new Apps.Github.Models.Respository.Requests.GetRepositoryRequest { RepositoryId= "1077915437" });

        var repositories = await handler.GetDataAsync(new DataSourceContext { SearchString = "" }, CancellationToken.None);

        PrintResult(repositories);
        Assert.IsNotNull(repositories);
    }

    private static void PrintResult(IEnumerable<DataSourceItem> items)
    {
        foreach (var item in items)
            Console.WriteLine($"{item.Value} - {item.DisplayName}");
    }
}
