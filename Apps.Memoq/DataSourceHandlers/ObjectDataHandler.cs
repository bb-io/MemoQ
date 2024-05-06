using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Apps.Memoq.Models.ServerProjects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using ResourceType = MQS.ServerProject.ResourceType;

namespace Apps.Memoq.DataSourceHandlers;

public class ObjectDataHandler : BaseInvocable, IAsyncDataSourceHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    private readonly string? _resourceType;

    public ObjectDataHandler(InvocationContext invocationContext, [ActionParameter] AddResourceToProjectRequest request)
        : base(invocationContext)
    {
        _resourceType = request.ResourceType;
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (_resourceType is null)
        {
            throw new InvalidOperationException("You should specify resource type first");
        }

        var resourceType = (ResourceType)int.Parse(_resourceType);
        if (resourceType == ResourceType.AutoCorrect ||
            resourceType == ResourceType.FilterConfigs ||
            resourceType == ResourceType.KeyboardShortcuts)
        {
            throw new InvalidOperationException(
                $"Resource type {resourceType} can not be assigned to server projects.");
        }

        if (resourceType == ResourceType.NonTrans || resourceType == ResourceType.LQA ||
            resourceType == ResourceType.MTSettings)
        {
            throw new InvalidOperationException("For this resource type you should not use Object ID property.");
        }

        if (resourceType == ResourceType.AutoTrans || resourceType == ResourceType.IgnoreLists ||
            resourceType == ResourceType.QASettings)
        {
            var targetLanguageDataHandler = new TargetLanguageDataHandler();
            return targetLanguageDataHandler.GetData();
        }

        if (resourceType == ResourceType.SegRules)
        {
            var targetLanguageDataHandler = new TargetLanguageDataHandler();
            return targetLanguageDataHandler.GetData();
        }

        if (resourceType == ResourceType.TMSettings)
        {
            var targetLanguageDataHandler = new TranslationMemoryDataHandler(InvocationContext);
            return await targetLanguageDataHandler.GetDataAsync(context, cancellationToken);
        }

        if (resourceType == ResourceType.PathRules)
        {
            return new Dictionary<string, string>()
            {
                { "File", "File" },
                { "Folder", "Folder" }
            };
        }
        
        throw new InvalidOperationException($"Resource type {resourceType} is not supported.");
    }
}