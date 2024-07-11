using Apps.Memoq.Contracts;
using Apps.Memoq.Models.Dto;
using Apps.Memoq.Models;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using MQS.TM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.MemoQ.DataSourceHandlers
{
    public class ProjectTemplateDataHandler : BaseInvocable, IAsyncDataSourceHandler
    {
        private IEnumerable<AuthenticationCredentialsProvider> Creds =>
            InvocationContext.AuthenticationCredentialsProviders;

        public ProjectTemplateDataHandler(InvocationContext invocationContext) : base(invocationContext)
        {
        }

        public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
            CancellationToken cancellationToken)
        {
            using var resourceService = new MemoqServiceFactory<IResourceService>(
                SoapConstants.ResourceServiceUrl, Creds);

            var response = await resourceService.Service.ListResourcesAsync(MQS.Resource.ResourceType.ProjectTemplate, new MQS.Resource.LightResourceListFilter { });

            return response
                .Where(x => context.SearchString is null ||
                             x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                .Take(20)
                .ToDictionary(x => x.Guid.ToString(), x => x.Name);
        }
    }
}
