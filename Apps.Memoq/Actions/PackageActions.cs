using Apps.Memoq.Models.Package.Request;
using Apps.Memoq.Models.Package.Response;
using Apps.MemoQ;
using Apps.MemoQ.Extensions;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Memoq.Actions;

[ActionList("Packages")]
public class PackageActions(InvocationContext invocationContext) : MemoqInvocable(invocationContext)
{
    [Action("Create delivery package", Description = "Create a new delivery package")]
    public async Task<CreateDeliveryPackageResponse> CreateDeliveryPackage([ActionParameter] CreateDeliveryPackageRequest input)
    {
        var projectId = GuidExtensions.ParseWithErrorHandling(input.ProjectGuid);
        var docIds = input.DocumentIds.Select((x) => GuidExtensions.ParseWithErrorHandling(x)).ToArray();

        var response = await ExecuteWithHandling(() => ProjectService.Service.CreateDeliveryPackageAsync(projectId, docIds,
            input.ReturnToPreviousActor ?? false));

        return new()
        {
            FileGuid = response.FileId.ToString(),
            DocumentsNotFinished = response.DocumentsNotFinished.Select(x => x.ToString())
        };
    }
    
    [Action("Deliver package", Description = "Deliver a specific package")]
    public async Task DeliverPackage([ActionParameter] PackageRequest input)
    {
         await ExecuteWithHandling(() => ProjectService.Service.DeliverPackageAsync(GuidExtensions.ParseWithErrorHandling(input.FileId)));
    }
}