namespace Blackbird.Applications.SDK.Blueprints
{    
    public enum BlueprintEvent
    {
        [BlueprintItemDefinition("https://blackbirdstoragelocal.blob.core.windows.net/test-files/thunder.png", "test webhook event", DefaultCategory = "test webhook event default category")]
        TestWebhookEvent,
        [BlueprintItemDefinition("pathToPollingIcon", "test polling event", DefaultCategory = "test polling event default category")]
        TestPollingEvent
    }
}
