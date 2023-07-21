namespace Apps.Github.Models.Commit.Requests
{
    public class UpdateFileRequest : PushFileRequest
    {
        public string FileId { get; set; }
    }
}
