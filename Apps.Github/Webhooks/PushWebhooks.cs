using Apps.Github.Webhooks.Handlers;
using Apps.Github.Webhooks.Payloads;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using System;
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
        public async Task<WebhookResponse<FilesListResponse>> FilesAddedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PushPayload>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }

            if (data.Commits.Any() && data.Commits.First().Added.Any())
            {
                return new WebhookResponse<FilesListResponse>
                {
                    HttpResponseMessage = null,
                    Result = new FilesListResponse() { AllFiles = data.Commits.First().Added, FirstFilename = data.Commits.First().Added.First() }
                };
            }
            return new WebhookResponse<FilesListResponse> { HttpResponseMessage = new HttpResponseMessage(statusCode: HttpStatusCode.OK) };
        }

        [Webhook("On files modified", typeof(PushActionHandler), Description = "On files modified")]
        public async Task<WebhookResponse<FilesListResponse>> FilesModifiedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PushPayload>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            if (data.Commits.Any() && data.Commits.First().Modified.Any())
            {
                return new WebhookResponse<FilesListResponse>
                {
                    HttpResponseMessage = null,
                    Result = new FilesListResponse() { AllFiles = data.Commits.First().Modified, FirstFilename = data.Commits.First().Modified.First() }
                };
            }
            return new WebhookResponse<FilesListResponse> { HttpResponseMessage = new HttpResponseMessage(statusCode: HttpStatusCode.OK) };
        }

        [Webhook("On files removed", typeof(PushActionHandler), Description = "On files removed")]
        public async Task<WebhookResponse<FilesListResponse>> FilesRemovedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PushPayload>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            if (data.Commits.Any() && data.Commits.First().Removed.Any())
            {
                return new WebhookResponse<FilesListResponse>
                {
                    HttpResponseMessage = null,
                    Result = new FilesListResponse() { AllFiles = data.Commits.First().Removed, FirstFilename = data.Commits.First().Removed.First() }
                };
            }
            return new WebhookResponse<FilesListResponse> { HttpResponseMessage = new HttpResponseMessage(statusCode: HttpStatusCode.OK) };
        }
    }
}
