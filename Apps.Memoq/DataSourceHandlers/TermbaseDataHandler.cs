using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.MemoQ;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using MQS.TB;

namespace Apps.Memoq.DataSourceHandlers;

public class TermbaseDataHandler(InvocationContext invocationContext) : MemoqInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, 
        CancellationToken cancellationToken)
    {
        var termbases = await TbService.Service.ListTBs2Async(null);

        return termbases
            .Where(termbase => context.SearchString is null ||
                               termbase.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(termbase => termbase.LastModified)
            .Take(20)
            .Select(termbase => new DataSourceItem(termbase.Guid.ToString(), termbase.IsQTerm ? $"{termbase.Name} (QTerm)" : termbase.Name));
    }
}