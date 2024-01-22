using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using MQS.TB;

namespace Apps.Memoq.DataSourceHandlers;

public class TermbaseDataHandler : BaseInvocable, IAsyncDataSourceHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public TermbaseDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, 
        CancellationToken cancellationToken)
    {
        using var tbService = new MemoqServiceFactory<ITBService>(SoapConstants.TermBasesServiceUrl, Creds);
        var termbases = await tbService.Service.ListTBs2Async(null);

        return termbases
            .Where(termbase => context.SearchString is null ||
                               termbase.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(termbase => termbase.LastModified)
            .Take(20)
            .ToDictionary(termbase => termbase.Guid.ToString(),
                termbase => termbase.IsQTerm ? $"{termbase.Name} (QTerm)" : termbase.Name);
    }
}