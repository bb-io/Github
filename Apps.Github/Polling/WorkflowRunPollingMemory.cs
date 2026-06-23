namespace Apps.GitHub.Polling;

public class WorkflowRunPollingMemory
{
    public string? LastKnownStatus { get; set; }

    public DateTime LastCheckedUtc { get; set; }

    public bool Triggered { get; set; }
}
