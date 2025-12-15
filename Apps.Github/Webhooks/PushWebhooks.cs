using Apps.Github.Webhooks.Handlers;
using Apps.Github.Webhooks.Payloads;
using Apps.GitHub.Webhooks.Payloads;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Microsoft.Extensions.FileSystemGlobbing;
using Newtonsoft.Json;
using System.Net;

namespace Apps.Github.Webhooks;

[WebhookList]
public class PushWebhooks
{
    //[Webhook("On commit pushed", typeof(PushActionHandler), Description = "On commit pushed")]
    //public async Task<WebhookResponse<PushPayloadFlat>> CommitPushedHandler(WebhookRequest webhookRequest, [WebhookParameter] BranchInput branchInput)
    //{
    //    var data = JsonConvert.DeserializeObject<PushPayload>(webhookRequest.Body.ToString());
    //    if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
    //    if (!string.IsNullOrEmpty(branchInput.BranchName) && branchInput.BranchName != data.Ref.Split('/').Last()) return GeneratePreflight<PushPayloadFlat>();

    //    return new()
    //    {
    //        HttpResponseMessage = null,
    //        Result = new(data)
    //    };
    //}

    // Todo: return a bit more information from the commit.

    [Webhook("On files added", typeof(PushActionHandler), Description = "On files added")]
    public async Task<WebhookResponse<FilesListResponse>> FilesAddedHandler(WebhookRequest webhookRequest,
        [WebhookParameter] FolderInput input, 
        [WebhookParameter] BranchInput branchInput)
    {
        var data = JsonConvert.DeserializeObject<PushPayload>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        if (!string.IsNullOrEmpty(branchInput.BranchName) && branchInput.BranchName != data.Ref.Split('/').Last()) return GeneratePreflight<FilesListResponse>();

        var folderPath = GetFolderPath(input);
        var addedFiles = new List<FilePathObj>();
        data.Commits.ForEach(c => addedFiles.AddRange(c.Added.Where(f => folderPath is null || IsFilePathMatchingPattern(folderPath, f))
            .Select(filePath => new FilePathObj { FilePath = filePath })));
        if (addedFiles.Any())
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = new()
                { 
                    Files = addedFiles, 
                }
            };
        }
        return GeneratePreflight<FilesListResponse>();
    }

    [Webhook("On files modified", typeof(PushActionHandler), Description = "On files modified")]
    public async Task<WebhookResponse<FilesListResponse>> FilesModifiedHandler(WebhookRequest webhookRequest,
        [WebhookParameter] FolderInput input, 
        [WebhookParameter] BranchInput branchInput)
    {
        var data = JsonConvert.DeserializeObject<PushPayload>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        if (!string.IsNullOrEmpty(branchInput.BranchName) && branchInput.BranchName != data.Ref.Split('/').Last()) return GeneratePreflight<FilesListResponse>();

        var folderPath = GetFolderPath(input);
        var modifiedFiles = new List<FilePathObj>();
        data.Commits.ForEach(c => modifiedFiles.AddRange(c.Modified.Where(f => folderPath is null || IsFilePathMatchingPattern(folderPath, f))
            .Select(filePath => new FilePathObj { FilePath = filePath })));
        if (modifiedFiles.Any())
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = new()
                { 
                    Files = modifiedFiles
                }
            };
        }
        return GeneratePreflight<FilesListResponse>();
    }

    [Webhook("On files added or modified", typeof(PushActionHandler), Description = "On files added or modified")]
    public async Task<WebhookResponse<FilesListResponse>> FilesAddedAndModifiedHandler(WebhookRequest webhookRequest,
        [WebhookParameter] FolderInput input, 
        [WebhookParameter] BranchInput branchInput)
    {
        var data = JsonConvert.DeserializeObject<PushPayload>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        if (!string.IsNullOrEmpty(branchInput.BranchName) && branchInput.BranchName != data.Ref.Split('/').Last()) return GeneratePreflight<FilesListResponse>();

        var folderPath = GetFolderPath(input);
        var files = new List<FilePathObj>();
        data.Commits.ForEach(c => {
            files.AddRange(c.Added.Where(f => folderPath is null || IsFilePathMatchingPattern(folderPath, f))
                .Select(fileId => new FilePathObj { FilePath = fileId }));
            files.AddRange(c.Modified.Where(f => folderPath is null || IsFilePathMatchingPattern(folderPath, f))
                .Select(fileId => new FilePathObj { FilePath = fileId }));
        });
        if (files.Any())
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = new()
                {
                    Files = files,
                }
            };
        }
        return GeneratePreflight<FilesListResponse>();
    }

    [Webhook("On files removed", typeof(PushActionHandler), Description = "On files removed")]
    public async Task<WebhookResponse<FilesListResponse>> FilesRemovedHandler(WebhookRequest webhookRequest,
        [WebhookParameter] FolderInput input, 
        [WebhookParameter] BranchInput branchInput)
    {
        var data = JsonConvert.DeserializeObject<PushPayload>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        if (!string.IsNullOrEmpty(branchInput.BranchName) && branchInput.BranchName != data.Ref.Split('/').Last()) return GeneratePreflight<FilesListResponse>();

        var folderPath = GetFolderPath(input);
        var removedFiles = new List<FilePathObj>();
        data.Commits.ForEach(c => removedFiles.AddRange(c.Removed.Where(f => folderPath is null || IsFilePathMatchingPattern(folderPath, f))
            .Select(filePath => new FilePathObj { FilePath = filePath })));
        if (removedFiles.Any())
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = new()
                { 
                    Files = removedFiles
                }
            };
        }
        return GeneratePreflight<FilesListResponse>();
    }
    public static bool IsFilePathMatchingPattern(string pattern, string filePath)
    {
        var matcher = new Matcher();
        matcher.AddInclude(pattern);
        
        return matcher.Match(filePath).HasMatches;
    }

    private WebhookResponse<T> GeneratePreflight<T>() where T : class
    {
        return new()
        {
            ReceivedWebhookRequestType = WebhookRequestType.Preflight,
            HttpResponseMessage = new(statusCode: HttpStatusCode.OK)
        };
    }

    private string? GetFolderPath(FolderInput input)
    {
        if (string.IsNullOrEmpty(input.FolderPath))
        {
            var recursivePath = !input.RecursivelyIncludeSubfolders.HasValue ? "/*" : (input.RecursivelyIncludeSubfolders.Value ? "/**" : "/*");
            if (!string.IsNullOrEmpty(input.Folder))
            {
                return input.Folder.TrimEnd('/') + recursivePath;
            }
            return null;
        }

        if (!string.IsNullOrEmpty(input.Folder))
        {
            return input.Folder.TrimEnd('/') + input.FolderPath;
        }

        return input.FolderPath;
    }
}