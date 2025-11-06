using Apps.Github.Models.Respository.Requests;
using Apps.GitHub.Models.Workflow;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json.Linq;

namespace Apps.GitHub.DataSourceHandlers
{
    public class WorkflowDataHandler(InvocationContext invocationContext, [ActionParameter] GetRepositoryRequest repositoryRequest)
     : GithubInvocable(invocationContext), IAsyncDataSourceItemHandler
    {
        private GetRepositoryRequest RepositoryRequest { get; } = repositoryRequest;

        public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context,CancellationToken cancellationToken)
        {
            if (RepositoryRequest == null || string.IsNullOrWhiteSpace(RepositoryRequest.RepositoryId))
                throw new PluginMisconfigurationException("Please, specify repository first");

            var repo = await ExecuteWithErrorHandlingAsync(async () =>
                await ClientSdk.Repository.Get(long.Parse(RepositoryRequest.RepositoryId)));

            var workflows = await GetAllWorkflowsViaRest(repo.Owner.Login, repo.Name);

            var search = context.SearchString?.Trim();
            if (!string.IsNullOrEmpty(search))
            {
                workflows = workflows.Where(w =>
                    (w.Name?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (w.Path?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false));
            }

            return workflows
                .OrderBy(w => w.Name ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                .Select(w =>
                {
                    var file = System.IO.Path.GetFileName(w.Path ?? string.Empty);
                    var label = string.IsNullOrEmpty(file) ? (w.Name ?? w.Id.ToString()) : $"{w.Name} ({file})";
                    return new DataSourceItem(w.Id.ToString(), label);
                })
                .ToList();
        }

        private async Task<IEnumerable<WorkflowLite>> GetAllWorkflowsViaRest(string owner, string repo)
        {
            var all = new List<WorkflowLite>();
            var page = 1;
            const int perPage = 100;

            while (true)
            {
                var endpoint = new Uri(
                    $"repos/{owner}/{repo}/actions/workflows?per_page={perPage}&page={page}",
                    UriKind.Relative);

                var response = await ExecuteWithErrorHandlingAsync(async () =>
                    await ClientSdk.Connection.Get<string>(endpoint, new Dictionary<string, string>()));

                var body = response?.Body ?? "{}";
                var json = JObject.Parse(body);

                var arr = json["workflows"] as JArray;
                if (arr == null || arr.Count == 0) break;

                var pageList = arr
                    .Select(w => new WorkflowLite
                    {
                        Id = (long?)(w?["id"]) ?? 0,
                        Name = (string?)(w?["name"]) ?? string.Empty,
                        Path = (string?)(w?["path"]) ?? string.Empty,
                        State = (string?)(w?["state"]) ?? "unknown"
                    })
                    .Where(x => x.Id != 0)
                    .ToList();

                if (pageList.Count == 0) break;

                all.AddRange(pageList);

                if (pageList.Count < perPage) break;
                page++;
            }

            return all;
        }
    }
}
