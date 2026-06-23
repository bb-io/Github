using Apps.GitHub.Models.Workflow;
using Newtonsoft.Json;

namespace Tests.Github;

[TestClass]
public class WorkflowModelsTests
{
    [TestMethod]
    public void WorkflowDispatchApiResponse_DeserializesRunIdentifiers()
    {
        const string json = """
                            {
                              "workflow_run_id": 12345,
                              "run_url": "https://api.github.com/repos/octo/repo/actions/runs/12345",
                              "html_url": "https://github.com/octo/repo/actions/runs/12345"
                            }
                            """;

        var response = JsonConvert.DeserializeObject<WorkflowDispatchApiResponse>(json);

        Assert.IsNotNull(response);
        Assert.AreEqual(12345, response.WorkflowRunId);
        Assert.AreEqual("https://api.github.com/repos/octo/repo/actions/runs/12345", response.RunUrl);
        Assert.AreEqual("https://github.com/octo/repo/actions/runs/12345", response.HtmlUrl);
    }

    [TestMethod]
    public void WorkflowRunApiResponse_DeserializesWorkflowRunState()
    {
        const string json = """
                            {
                              "id": 12345,
                              "workflow_id": 999,
                              "name": "UI tests",
                              "display_title": "Run UI tests",
                              "event": "workflow_dispatch",
                              "status": "completed",
                              "conclusion": "success",
                              "head_branch": "main",
                              "head_sha": "abc123",
                              "run_attempt": 1,
                              "url": "https://api.github.com/repos/octo/repo/actions/runs/12345",
                              "html_url": "https://github.com/octo/repo/actions/runs/12345",
                              "jobs_url": "https://api.github.com/repos/octo/repo/actions/runs/12345/jobs",
                              "logs_url": "https://api.github.com/repos/octo/repo/actions/runs/12345/logs",
                              "created_at": "2026-06-22T10:00:00Z",
                              "updated_at": "2026-06-22T10:02:00Z",
                              "run_started_at": "2026-06-22T10:00:10Z"
                            }
                            """;

        var response = JsonConvert.DeserializeObject<WorkflowRunApiResponse>(json);

        Assert.IsNotNull(response);
        Assert.AreEqual(12345, response.Id);
        Assert.AreEqual(999, response.WorkflowId);
        Assert.AreEqual("completed", response.Status);
        Assert.AreEqual("success", response.Conclusion);
        Assert.AreEqual("workflow_dispatch", response.Event);
        Assert.AreEqual("main", response.HeadBranch);
    }
}
