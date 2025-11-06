namespace Apps.GitHub.Models.Workflow
{
    public sealed class WorkflowLite
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public string Path { get; set; } = "";
        public string State { get; set; } = "";
    }
}
