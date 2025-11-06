using Apps.GitHub.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Newtonsoft.Json;

namespace Apps.GitHub.Models.Workflow
{
    public class TriggerWorkflowRequest
    {
        [Display("Workflow")]
        [JsonProperty("workflow")]
        [DataSource(typeof(WorkflowDataHandler))]
        public string Workflow { get; set; } = default!;

        [Display("Ref", Description = " The reference can be a branch or tag name")]
        [DataSource(typeof(BranchDataHandler))]
        [JsonProperty("ref")]
        public string Ref { get; set; } = "main";

        [Display("Inputs (JSON)")]
        [JsonProperty("inputs")]
        public string? InputsJson { get; set; }
    }
}
