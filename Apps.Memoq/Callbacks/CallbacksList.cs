using Apps.Memoq.Callbacks.Payload;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using System.Text.Json;

namespace Apps.Memoq.Webhooks
{
    [WebhookList]
    public class CallbacksList 
    {
        #region Deliver callbacks

        [Webhook("On document delivered", Description = "On document delivered")]
        public async Task<WebhookResponse<ProjectDeliveredDto>> ProjectCreation(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<ProjectDeliveredDto>(webhookRequest.Body.ToString());
            if(data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return new WebhookResponse<ProjectDeliveredDto>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        #endregion
    }
}
