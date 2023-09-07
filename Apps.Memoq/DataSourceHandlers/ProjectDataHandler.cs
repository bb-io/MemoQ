using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using MQS.ServerProject;

namespace Apps.Memoq.DataSourceHandlers;

public class ProjectDataHandler : BaseInvocable, IDataSourceHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public ProjectDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public Dictionary<string, string> GetData(DataSourceContext context)
    {
        var projectService = new MemoqServiceFactory<IServerProjectService>(SoapConstants.ProjectServiceUrl,
            Creds);
        var projects = projectService.Service.ListProjects(new ServerProjectListFilter());

        return projects
            .Where(x => context.SearchString is null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.CreationTime)
            .Take(20)
            .ToDictionary(x => x.ServerProjectGuid.ToString(), x => x.Name);
    }
}