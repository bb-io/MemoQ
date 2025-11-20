using Apps.MemoQ;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using DocumentFormat.OpenXml.Spreadsheet;
using MQS.ServerProject;
using Newtonsoft.Json;
using System.Text;

namespace Apps.Memoq.DataSourceHandlers;

public class ProjectDataHandler : MemoqInvocable, IAsyncDataSourceItemHandler
{
    public ProjectDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var projects = await ProjectService.Service.ListProjectsAsync(new ServerProjectListFilter());

        return projects
            .Where(x => context.SearchString is null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.CreationTime)
            .Take(20)
            .Select(x => new DataSourceItem(x.ServerProjectGuid.ToString(), x.Name));
    }
}