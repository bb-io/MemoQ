using Apps.Memoq.Callbacks.Handlers;
using Apps.Memoq.Callbacks.Models.Payload.Base;
using Apps.Memoq.Callbacks.Models.Response;
using Apps.MemoQ;
using Apps.MemoQ.Callbacks.Handlers;
using Apps.MemoQ.Callbacks.Models.Request;
using Apps.MemoQ.Callbacks.Models.Response;
using Apps.MemoQ.Extensions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using System.Net;
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

    [Webhook("On all files delivered", typeof(AllFilesDeliveredHandler),
        Description = "Triggered when ALL files of the specified project are delivered. Emits preflight until then.")]
    public async Task<WebhookResponse<AllFilesDeliveredResponse>> OnAllFilesDelivered(
        WebhookRequest webhookRequest,
        [WebhookParameter] AllFilesDeliveredInput input)
    {
        InvocationContext.Logger?.LogInformation(
        "[MemoQ][OnAllFilesDelivered] Incoming callback", null);

        if (string.IsNullOrWhiteSpace(input.ProjectId))
        {
            InvocationContext.Logger?.LogInformation(
                "[MemoQ][OnAllFilesDelivered] No project ID provided → preflight", null);
            return Preflight<AllFilesDeliveredResponse>();
        }

        if (string.IsNullOrWhiteSpace(webhookRequest.Body?.ToString()))
        {
            InvocationContext.Logger?.LogInformation(
                "[MemoQ][OnAllFilesDelivered] Empty body → preflight", null);
            return Preflight<AllFilesDeliveredResponse>();
        }

        try
        {
            var serializer = new XmlSerializer(typeof(Envelope));
            using TextReader reader = new StringReader(webhookRequest.Body!.ToString()!);
            var envelope = (Envelope)serializer.Deserialize(reader)!;

        }
        catch (Exception ex)
        {
            InvocationContext.Logger?.LogInformation(
                $"[MemoQ][OnAllFilesDelivered] Failed to deserialize envelope → treat as preflight. Error: {ex.Message}",
                null);
            return Preflight<AllFilesDeliveredResponse>();
        }

        var projectGuid = GuidExtensions.ParseWithErrorHandling(input.ProjectId);

        var documents = await ExecuteWithHandling(() =>
            ProjectService.Service.ListProjectTranslationDocuments2Async(
                projectGuid,
                new()
                {
                    FillInAssignmentInformation = false
                }));

        var notDelivered = documents
            .Where(d => !IsDelivered(d))
            .ToList();

        if (notDelivered.Any())
        {
            InvocationContext.Logger?.LogInformation(
                $"[MemoQ][OnAllFilesDelivered] Still not delivered: {string.Join(", ", notDelivered.Select(d => d.DocumentName))}",
                null);

            return Preflight<AllFilesDeliveredResponse>();
        }

        var result = new AllFilesDeliveredResponse
        {
            ProjectId = input.ProjectId,
            Documents = documents.Select(d => new AllFilesDeliveredDocumentDto
            {
                Id = d.DocumentGuid.ToString(),
                Name = d.DocumentName,
                IsDelivered = true,
                TargetLanguage = d.TargetLangCode
            }).ToList()
        };

        return new WebhookResponse<AllFilesDeliveredResponse>
        {
            HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
            ReceivedWebhookRequestType = WebhookRequestType.Default,
            Result = result
        };
    }
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

    private static WebhookResponse<T> Preflight<T>() where T : class =>
        new()
        {
            HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
            ReceivedWebhookRequestType = WebhookRequestType.Preflight
        };

    private static bool IsDelivered(dynamic doc)
    {
        return doc.IsDelivered;
    }
}