using Apps.Memoq.Callbacks.Handlers.Base;
using Apps.MemoQ.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Memoq.Callbacks.Handlers;

public class DocumentDeliveredHandler : WebhookHandler
{
    public DocumentDeliveredHandler(InvocationContext invocationContext, [WebhookParameter] TestBlueprintInput testBlueprint) : base(invocationContext)
    {
    }

    protected override string Event => "DocumentDelivery";
}