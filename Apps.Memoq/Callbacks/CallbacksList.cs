using System.Xml.Serialization;
using Apps.Memoq.Callbacks.Handlers;
using Apps.Memoq.Callbacks.Models.Payload.Base;
using Apps.Memoq.Callbacks.Models.Response;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Memoq.Callbacks;

[WebhookList]
public class CallbacksList
{
    #region Deliver callbacks

    [Webhook("On document delivered (manual)", Description = "On a specific document delivered (manual)")]
    public Task<WebhookResponse<DocumentDeliveredResponse>> OnDocumentDeliveredManual(WebhookRequest webhookRequest)
        => HandleCallback(webhookRequest);

    [Webhook("On document delivered", typeof(DocumentDeliveredHandler),
        Description = "On a specific document delivered")]
    public Task<WebhookResponse<DocumentDeliveredResponse>> OnDocumentDelivered(WebhookRequest webhookRequest)
        => HandleCallback(webhookRequest);

    #endregion

    private Task<WebhookResponse<DocumentDeliveredResponse>> HandleCallback(WebhookRequest webhookRequest)
    {
        ArgumentException.ThrowIfNullOrEmpty(webhookRequest.Body.ToString(), nameof(webhookRequest.Body));

        var serializer = new XmlSerializer(typeof(Envelope));
        using TextReader reader = new StringReader(webhookRequest.Body.ToString()!);
       
        var envelope = (Envelope)serializer.Deserialize(reader)!;
        var result = new DocumentDeliveredResponse(envelope.Body.DocumentDelivery);

        return Task.FromResult(new WebhookResponse<DocumentDeliveredResponse>
        {
            HttpResponseMessage = null,
            Result = result
        });
    }
}