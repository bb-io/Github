﻿using Apps.Github.Webhooks.Payloads;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Github.Webhooks.Handlers;

public class PushActionHandler(
    InvocationContext invocationContext,
    [WebhookParameter(true)] WebhookRepositoryInput input)
    : BaseWebhookHandler(invocationContext, input, SubscriptionEvent)
{
    const string SubscriptionEvent = "push";
}