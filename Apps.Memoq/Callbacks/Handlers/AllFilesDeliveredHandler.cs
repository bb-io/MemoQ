using Apps.Memoq.Callbacks.Handlers.Base;
using Apps.MemoQ.Callbacks.Models.Request;
using Apps.MemoQ.Callbacks.Models.Response;
using Apps.MemoQ.Extensions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.MemoQ.Callbacks.Handlers
{
    public class AllFilesDeliveredHandler : WebhookHandler, IAfterSubscriptionWebhookEventHandler<AllFilesDeliveredResponse>
    {
        protected override string Event => "DocumentDelivered";
        private readonly AllFilesDeliveredInput input;

        public AllFilesDeliveredHandler(InvocationContext invocationContext, [WebhookParameter] AllFilesDeliveredInput input) : base(invocationContext)
        {
            this.input = input;
        }

        public async Task<AfterSubscriptionEventResponse<AllFilesDeliveredResponse>> OnWebhookSubscribedAsync()
        {
            InvocationContext.Logger?.LogInformation(
                "[MemoQ][OnAllFilesDelivered][AfterSub] Start after-subscription check", null);

            if (string.IsNullOrWhiteSpace(input.ProjectId))
            {
                InvocationContext.Logger?.LogInformation(
                    "[MemoQ][OnAllFilesDelivered][AfterSub] No project ID provided → nothing to emit", null);
                return null!;
            }

            var projectGuid = GuidExtensions.ParseWithErrorHandling(input.ProjectId);

            var docs = await ExecuteWithHandling(() => ProjectService.Service
                .ListProjectTranslationDocuments2Async(projectGuid, new()
                {
                    FillInAssignmentInformation = false
                }));

            var docList = docs.Cast<dynamic>().ToList();

            var notDelivered = docList
                .Where(d => !IsDelivered(d))
                .ToList();

            if (notDelivered.Any())
            {
                InvocationContext.Logger?.LogInformation(
                    $"[MemoQ][OnAllFilesDelivered][AfterSub] Not all docs delivered → count: {notDelivered.Count}",
                    null);
                return null!;
            }

            var result = new AllFilesDeliveredResponse
            {
                ProjectId = input.ProjectId,
                Documents = docList.Select(d => new AllFilesDeliveredDocumentDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Status = d.DocumentStatus.ToString(),
                    TargetLanguage = d.TargetLanguage
                }).ToList()
            };

            return new AfterSubscriptionEventResponse<AllFilesDeliveredResponse>
            {
                Result = result
            };
        }

        private static bool IsDelivered(dynamic doc)
        {
            try
            {
                return doc.IsDelivered;
            }
            catch
            {
                return false;
            }
        }
    }
}
