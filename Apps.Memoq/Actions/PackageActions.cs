using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.Memoq.Models.Package.Request;
using Apps.Memoq.Models.Package.Response;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using MQS.ServerProject;

namespace Apps.Memoq.Actions;

[ActionList]
public class PackageActions : BaseInvocable
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public PackageActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("Create delivery package", Description = "Create a new delivery package")]
    public async Task<CreateDeliveryPackageResponse> CreateDeliveryPackage([ActionParameter] CreateDeliveryPackageRequest input)
    {
        var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        var projectId = Guid.Parse(input.ProjectGuid);
        var docIds = input.DocumentIds.Select(Guid.Parse).ToArray();

        var response = await projectService.Service.CreateDeliveryPackageAsync(projectId, docIds,
            input.ReturnToPreviousActor ?? false);

        return new()
        {
            FileGuid = response.FileId.ToString(),
            DocumentsNotFinished = response.DocumentsNotFinished.Select(x => x.ToString())
        };
    }
    
    [Action("Deliver package", Description = "Deliver specific package")]
    public async Task DeliverPackage([ActionParameter] PackageRequest input)
    {
        var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

         await projectService.Service.DeliverPackageAsync(Guid.Parse(input.FileId));
    }
}