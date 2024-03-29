﻿using Apps.Github.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.Webhooks.Payloads;

public class WebhookRepositoryInput
{
    [Display("Repository")]
    [DataSource(typeof(RepositoryDataHandler))]
    public string RepositoryId { get; set; }
}