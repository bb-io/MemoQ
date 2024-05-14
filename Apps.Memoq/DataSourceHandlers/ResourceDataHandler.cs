using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.Memoq.Models.ServerProjects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using MQS.Resource;

namespace Apps.Memoq.DataSourceHandlers;

public class ResourceDataHandler : BaseInvocable, IAsyncDataSourceHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;
    private readonly string? _resourceType;

    public ResourceDataHandler(InvocationContext invocationContext, [ActionParameter] AddResourceToProjectRequest request) : base(invocationContext)
    {
        _resourceType = request.ResourceType;
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        if (_resourceType is null)
        {
            throw new InvalidOperationException("You should specify resource type first");
        }
        
        var resourceService = new MemoqServiceFactory<IResourceService>(
            SoapConstants.ResourceServiceUrl, Creds);

        var resourceType = (ResourceType)int.Parse(_resourceType);
        var resources = await resourceService.Service.ListResourcesAsync(resourceType, new LightResourceListFilter());
        
        return resources
            .Where(x => context.SearchString is null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(20)
            .ToDictionary(x => x.Guid.ToString(), x => x.Name);
    }
}