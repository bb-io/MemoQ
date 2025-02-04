using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;

namespace Apps.MemoQ.DataSourceHandlers
{
    public class ProjectTemplateDataHandler : MemoqInvocable, IAsyncDataSourceHandler
    {
        public ProjectTemplateDataHandler(InvocationContext invocationContext) : base(invocationContext)
        {
        }

        public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
            CancellationToken cancellationToken)
        {
            var response = await ResourceService.Service.ListResourcesAsync(MQS.Resource.ResourceType.ProjectTemplate, new MQS.Resource.LightResourceListFilter { });

            return response
                .Where(x => context.SearchString is null ||
                             x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                .Take(20)
                .ToDictionary(x => x.Guid.ToString(), x => x.Name);
        }
    }
}
