using Apps.GitHub.Api;
using Apps.GitHub.Dtos;
using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Tests.Github;

[TestClass]
public class WorkflowDispatchErrorMapperTests
{
    [TestMethod]
    public void Map_ForbiddenForPersonalAccessToken_ReturnsMisconfigurationWithRequiredPermission()
    {
        var error = new GithubErrorException(403, "Resource not accessible by personal access token");

        var mapped = WorkflowDispatchErrorMapper.Map(error);

        Assert.IsInstanceOfType<PluginMisconfigurationException>(mapped);
        StringAssert.Contains(mapped.Message, "Resource not accessible by personal access token");
        StringAssert.Contains(mapped.Message, "Actions");
        StringAssert.Contains(mapped.Message, "Read and write");
    }

    [TestMethod]
    public void Map_OtherError_ReturnsApplicationExceptionWithOriginalMessage()
    {
        var error = new GithubErrorException(422, "Unexpected inputs provided");

        var mapped = WorkflowDispatchErrorMapper.Map(error);

        Assert.IsInstanceOfType<PluginApplicationException>(mapped);
        Assert.AreEqual("Unexpected inputs provided", mapped.Message);
    }
}
