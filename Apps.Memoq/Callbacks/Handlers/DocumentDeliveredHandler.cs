using Apps.Memoq.Callbacks.Handlers.Base;
using Apps.Memoq.Models.ServerProjects.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Memoq.Callbacks.Handlers;

public class DocumentDeliveredHandler : WebhookHandler
{
    protected override string Event => "DocumentDelivery";

    public DocumentDeliveredHandler([WebhookParameter] ProjectRequest project) : base(project)
    {
    }
}