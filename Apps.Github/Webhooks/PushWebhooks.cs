using Apps.Github.Webhooks.Handlers;
using Apps.Github.Webhooks.Payloads;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace Apps.Github.Webhooks
{
    [WebhookList]
    public class PushWebhooks
    {
        [Webhook("On commit pushed", typeof(PushActionHandler), Description = "On commit pushed")]
        public async Task<WebhookResponse<PushPayloadFlat>> CommitPushedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PushPayload>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PushPayloadFlat>
            {
                HttpResponseMessage = null,
                Result = new PushPayloadFlat(data)
            };
        }

        [Webhook("On files added", typeof(PushActionHandler), Description = "On files added")]
        public async Task<WebhookResponse<FilesListResponse>> FilesAddedHandler(WebhookRequest webhookRequest,
            [WebhookParameter] FolderInput input)
        {
            var data = JsonConvert.DeserializeObject<PushPayload>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }

            var addedFiles = new List<FileId>();
            data.Commits.ForEach(c => addedFiles.AddRange(c.Added.Where(f => input.FolderPath is null || f.Contains(input.FolderPath))
                .Select(fileId => new FileId() { FilePath = fileId })));
            if (addedFiles.Any())
            {
                return new WebhookResponse<FilesListResponse>
                {
                    HttpResponseMessage = null,
                    Result = new FilesListResponse() { 
                        Files = addedFiles, 
                    }
                };
            }
            return new WebhookResponse<FilesListResponse> { HttpResponseMessage = new HttpResponseMessage(statusCode: HttpStatusCode.OK) };
        }

        [Webhook("On files modified", typeof(PushActionHandler), Description = "On files modified")]
        public async Task<WebhookResponse<FilesListResponse>> FilesModifiedHandler(WebhookRequest webhookRequest,
            [WebhookParameter] FolderInput input)
        {
            var data = JsonConvert.DeserializeObject<PushPayload>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }

            var modifiedFiles = new List<FileId>();
            data.Commits.ForEach(c => modifiedFiles.AddRange(c.Modified.Where(f => input.FolderPath is null || f.Contains(input.FolderPath))
                .Select(fileId => new FileId() { FilePath = fileId })));
            if (modifiedFiles.Any())
            {
                return new WebhookResponse<FilesListResponse>
                {
                    HttpResponseMessage = null,
                    Result = new FilesListResponse() { 
                        Files = modifiedFiles
                    }
                };
            }
            return new WebhookResponse<FilesListResponse> { HttpResponseMessage = new HttpResponseMessage(statusCode: HttpStatusCode.OK) };
        }

        [Webhook("On files added and modified", typeof(PushActionHandler), Description = "On files added and modified")]
        public async Task<WebhookResponse<FilesListResponse>> FilesAddedAndModifiedHandler(WebhookRequest webhookRequest,
            [WebhookParameter] FolderInput input)
        {
            var data = JsonConvert.DeserializeObject<PushPayload>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }

            var files = new List<FileId>();
            data.Commits.ForEach(c => {
                files.AddRange(c.Added.Where(f => input.FolderPath is null || f.Contains(input.FolderPath))
                    .Select(fileId => new FileId() { FilePath = fileId }));
                files.AddRange(c.Modified.Where(f => input.FolderPath is null || f.Contains(input.FolderPath))
                    .Select(fileId => new FileId() { FilePath = fileId }));
            });
            if (files.Any())
            {
                return new WebhookResponse<FilesListResponse>
                {
                    HttpResponseMessage = null,
                    Result = new FilesListResponse()
                    {
                        Files = files,
                    }
                };
            }
            return new WebhookResponse<FilesListResponse> { HttpResponseMessage = new HttpResponseMessage(statusCode: HttpStatusCode.OK) };
        }

        [Webhook("On files removed", typeof(PushActionHandler), Description = "On files removed")]
        public async Task<WebhookResponse<FilesListResponse>> FilesRemovedHandler(WebhookRequest webhookRequest,
            [WebhookParameter] FolderInput input)
        {
            var data = JsonConvert.DeserializeObject<PushPayload>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }

            var removedFiles = new List<FileId>();
            data.Commits.ForEach(c => removedFiles.AddRange(c.Removed.Where(f => input.FolderPath is null || f.Contains(input.FolderPath))
                .Select(fileId => new FileId() { FilePath = fileId })));
            if (removedFiles.Any())
            {
                return new WebhookResponse<FilesListResponse>
                {
                    HttpResponseMessage = null,
                    Result = new FilesListResponse() { 
                        Files = removedFiles
                    }
                };
            }
            return new WebhookResponse<FilesListResponse> { HttpResponseMessage = new HttpResponseMessage(statusCode: HttpStatusCode.OK) };
        }
    }
}
