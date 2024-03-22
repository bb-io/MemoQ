using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.Memoq.Models.Dto;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using MQS.TM;

namespace Apps.Memoq.DataSourceHandlers;

public class TranslationMemoryDataHandler : BaseInvocable, IAsyncDataSourceHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public TranslationMemoryDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, 
        CancellationToken cancellationToken)
    {
        using var tmService = new MemoqServiceFactory<ITMService>(
            SoapConstants.TranslationMemoryServiceUrl, Creds);

        var response = await tmService.Service.ListTMs2Async(new TMListFilter());
        var translationMemories = response.Select(x => new TmDto(x)).ToArray();
        
        return translationMemories
            .Where(tm => context.SearchString is null ||
                         BuildReadableName(tm).Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(tm => tm.LastModified)
            .Take(20)
            .ToDictionary(tm => tm.Guid.ToString(), BuildReadableName);
    }
    
    private string BuildReadableName(TmDto tm)
    {
        return $"{tm.Project} [{tm.SourceLanguageCode} - {tm.TargetLanguageCode}]";
    }
}