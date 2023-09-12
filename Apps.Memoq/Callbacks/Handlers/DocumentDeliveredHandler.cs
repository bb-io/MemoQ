using Apps.Memoq.Callbacks.Handlers.Base;

namespace Apps.Memoq.Callbacks.Handlers;

public class DocumentDeliveredHandler : WebhookHandler
{
    protected override string Event => "DocumentDelivery";
}