using Apps.Github.DataSourceHandlers;
using Apps.GitHub.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Tests.Github.Base;

namespace Tests.Github
{
    [TestClass]
    public class DataHandlerTests : TestBase
    {
        [TestMethod]
        public async Task RepositoryDataHandler_IsSuccess()
        {
            var handler = new RepositoryDataHandler(InvocationContext);

            var repositories = await handler.GetDataAsync(new DataSourceContext { SearchString=""}, CancellationToken.None);

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(repositories);

            Console.WriteLine(json);

            Assert.IsNotNull(repositories);
        }

        [TestMethod]
        public async Task WorkflowDataHandler_IsSuccess()
        {
            var handler = new WorkflowDataHandler(InvocationContext, new Apps.Github.Models.Respository.Requests.GetRepositoryRequest { RepositoryId= "1077915437" });

            var repositories = await handler.GetDataAsync(new DataSourceContext { SearchString = "" }, CancellationToken.None);

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(repositories);

            Console.WriteLine(json);

            Assert.IsNotNull(repositories);
        }
    }
}
