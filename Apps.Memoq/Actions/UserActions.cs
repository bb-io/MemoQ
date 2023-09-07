using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.Memoq.Models.Dto;
using Apps.Memoq.Models.Users.Requests;
using Apps.Memoq.Models.Users.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using MQS.Security;

namespace Apps.Memoq.Actions;

[ActionList]
public class UserActions : BaseInvocable
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;
        
    public UserActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }
        
    [Action("List all users", Description = "List all users")]
    public ListAllUsersResponse ListAllUsers()
    {
        var securityService = new MemoqServiceFactory<ISecurityService>(
            SoapConstants.SecurityServiceUrl, Creds);

        var response = securityService.Service.ListUsers();
        return new()
        {
            Users = response.Select(x => new UserDto(x)).ToArray()
        };
    }

    [Action("Get user", Description = "Get user by guid")]
    public UserDto GetUser([ActionParameter] UserRequest user)
    {
        var securityService = new MemoqServiceFactory<ISecurityService>(
            SoapConstants.SecurityServiceUrl, Creds);
            
        var response = securityService.Service.GetUser(Guid.Parse(user.UserGuid));
        return new(response);
    }

    [Action("Delete user", Description = "Delete user by guid")]
    public void DeleteUser([ActionParameter] UserRequest user)
    {
        var securityService = new MemoqServiceFactory<ISecurityService>(
            SoapConstants.SecurityServiceUrl, Creds);
            
        securityService.Service.DeleteUser(Guid.Parse(user.UserGuid));
    }
}