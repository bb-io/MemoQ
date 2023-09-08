using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.Memoq.Models.Dto;
using Apps.Memoq.Models.Group.Response;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using MQS.Security;

namespace Apps.Memoq.Actions;

[ActionList]
public class GroupActions : BaseInvocable
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public GroupActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }
    
    [Action("List groups", Description = "List all groups")]
    public async Task<ListGroupsResponse> ListGroups()
    {
        var securityService = new MemoqServiceFactory<ISecurityService>(
            SoapConstants.SecurityServiceUrl, Creds);

        var response = await securityService.Service.ListGroupsAsync();
        var groups = response.Select(x => new GroupDto(x)).ToArray();

        return new(groups);
    }
}