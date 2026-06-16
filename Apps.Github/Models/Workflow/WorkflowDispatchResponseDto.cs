namespace Apps.GitHub.Models.Workflow
{
    public class WorkflowDispatchResponseDto
    {
        public string Repository { get; set; } = default!;
        public string Workflow { get; set; } = default!;
        public string Ref { get; set; } = default!;
        public string? InputsJson { get; set; }
        public string Status { get; set; } = "requested";
    }
}
