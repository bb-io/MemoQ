using Apps.Memoq.Callbacks.Handlers;
using Apps.Memoq.Callbacks.Models.Payload.Base;
using Apps.Memoq.Callbacks.Models.Response;
using Apps.MemoQ;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using System.Xml.Serialization;

namespace Apps.Memoq.Callbacks;

[WebhookList]
public class CallbacksList(InvocationContext invocationContext) : MemoqInvocable(invocationContext)
{
    #region Deliver callbacks

    [Webhook("On file delivered (manual)", Description = "On a specific file delivered (manual)")]
    public Task<WebhookResponse<DocumentDeliveredResponse>> OnDocumentDeliveredManual(WebhookRequest webhookRequest)
        => HandleCallback(webhookRequest);

    [Webhook("On file delivered", typeof(DocumentDeliveredHandler),
        Description = "On a specific file delivered")]
    public Task<WebhookResponse<DocumentDeliveredResponse>> OnDocumentDelivered(WebhookRequest webhookRequest)
        => HandleCallback(webhookRequest);

    #endregion

    private Task<WebhookResponse<DocumentDeliveredResponse>> HandleCallback(WebhookRequest webhookRequest)
    {
        try
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
        catch (Exception ex)
        {
            var errorMessage = "[MemoQ callback] Got an error while processing the callback request. "
                + $"Request method: {webhookRequest.HttpMethod?.Method}"
                + $"Request body: {webhookRequest.Body}"
                + $"Exception message: {ex.Message}";

            InvocationContext.Logger?.LogError(errorMessage, [ex.Message]);
            throw;
        }

    }
}