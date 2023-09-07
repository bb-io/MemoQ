using Apps.Memoq.Models.ServerProjects.Requests;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Blackbird.Applications.Sdk.Utils.Webhooks.Bridge;
using Blackbird.Applications.Sdk.Utils.Webhooks.Bridge.Models.Request;

namespace Apps.Memoq.Callbacks.Handlers.Base;

public abstract class WebhookHandler : IWebhookEventHandler
{
    protected abstract string Event { get; }

    private string ProjectGuid { get; }

    protected WebhookHandler([WebhookParameter] ProjectRequest project)
    {
        ProjectGuid = project.ProjectGuid;
    }

    public Task SubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values)
    {
        var (webhookData, bridgeCreds) = GetBridgeServiceInputs(values);
        return BridgeService.Subscribe(webhookData, bridgeCreds);
    }

    public Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values)
    {
        var (webhookData, bridgeCreds) = GetBridgeServiceInputs(values);
        return BridgeService.Unsubscribe(webhookData, bridgeCreds);
    }

    private (BridgeRequest webhookData, BridgeCredentials bridgeCreds) GetBridgeServiceInputs(
        Dictionary<string, string> values)
    {
        var webhookData = new BridgeRequest
        {
            Event = Event,
            Id = ProjectGuid,
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