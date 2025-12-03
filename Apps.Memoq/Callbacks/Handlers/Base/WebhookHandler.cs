using Apps.Memoq.Extensions;
using Apps.MemoQ;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Blackbird.Applications.Sdk.Utils.Webhooks.Bridge;
using Blackbird.Applications.Sdk.Utils.Webhooks.Bridge.Models.Request;

namespace Apps.Memoq.Callbacks.Handlers.Base;

public class WebhookHandler : MemoqInvocable, IWebhookEventHandler
{
    public WebhookHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    protected virtual string Event { get; }

    protected (BridgeRequest webhookData, BridgeCredentials bridgeCreds) GetBridgeServiceInputs(
        Dictionary<string, string> values,
        IEnumerable<AuthenticationCredentialsProvider> creds)
    {
        var webhookData = new BridgeRequest
        {
            Event = Event,
            Id = creds.GetInstanceUrlHash(),
            Url = values["payloadUrl"],
        };

        var bridgeCreds = new BridgeCredentials
        {
            ServiceUrl = $"{InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}{ApplicationConstants.MemoqBridgePath}",
            Token = ApplicationConstants.BlackbirdToken
        };

        return (webhookData, bridgeCreds);
    }

    public async Task SubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> creds, Dictionary<string, string> values)
    {
        var (input, creds2) = GetBridgeServiceInputs(values, creds);
        await BridgeService.Subscribe(input, creds2);
    }

    public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> creds, Dictionary<string, string> values)
    {
        var (input, creds2) = GetBridgeServiceInputs(values, creds);
        await BridgeService.Unsubscribe(input, creds2);
    }
}