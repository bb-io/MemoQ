using Apps.Memoq.Extensions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Webhooks.Bridge;
using Blackbird.Applications.Sdk.Utils.Webhooks.Bridge.Models.Request;

namespace Apps.Memoq.Callbacks.Handlers.Base;

public abstract class WebhookHandler : BridgeWebhookHandler
{
    protected abstract string Event { get; }

    protected override (BridgeRequest webhookData, BridgeCredentials bridgeCreds) GetBridgeServiceInputs(
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
            ServiceUrl = ApplicationConstants.BridgeServiceUrl,
            Token = ApplicationConstants.BlackbirdToken
        };

        return (webhookData, bridgeCreds);
    }
}