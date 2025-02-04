﻿using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.Memoq.Models.ServerProjects.Requests;
using Apps.MemoQ;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using MQS.Resource;

namespace Apps.Memoq.DataSourceHandlers;

public class ResourceDataHandler : MemoqInvocable, IAsyncDataSourceItemHandler
{
    private readonly string? _resourceType;

    public ResourceDataHandler(InvocationContext invocationContext, [ActionParameter] AddResourceToProjectRequest request) : base(invocationContext)
    {
        _resourceType = request.ResourceType;
    }

    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        if (_resourceType is null)
        {
            throw new InvalidOperationException("You should specify resource type first");
        }
        var resourceType = (ResourceType)int.Parse(_resourceType);
        var resources = await ResourceService.Service.ListResourcesAsync(resourceType, new LightResourceListFilter());
        
        return resources
            .Where(x => context.SearchString is null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(20)
            .Select(x => new DataSourceItem(x.Guid.ToString(), x.Name));
    }
}