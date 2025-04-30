using Apps.Memoq.Callbacks.Handlers.Base;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Memoq.Callbacks.Handlers;

public class DocumentDeliveredHandler : WebhookHandler
{
    public DocumentDeliveredHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    protected override string Event => "DocumentDelivery";
}