using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.Memoq.Models.Files.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using MQS.ServerProject;

namespace Apps.Memoq.DataSourceHandlers;

public class DocumentDataHandler : BaseInvocable, IDataSourceHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    private readonly string _projectGuid;

    public DocumentDataHandler(InvocationContext invocationContext, [ActionParameter] GetDocumentRequest request) :
        base(invocationContext)
    {
        _projectGuid = request.ProjectGuid;
    }

    public Dictionary<string, string> GetData(DataSourceContext context)
    {
        if (string.IsNullOrEmpty(_projectGuid))
        {
            throw new InvalidOperationException("You should input a project guid first");
        }

        using var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        var response = projectService.Service.ListProjectTranslationDocumentsGroupedBySourceFile(
            Guid.Parse(_projectGuid));

        var files = response
            .SelectMany(x => x.Groups)
            .SelectMany(x => x.Documents)
            .ToList();
        
        return files
            .Where(x => context.SearchString is null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.Name)
            .Take(20)
            .ToDictionary(x => x.DocumentGuid.ToString(), x => x.Name);
    }
}