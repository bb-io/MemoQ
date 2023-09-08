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

    [Action("Create user", Description = "Create a new user")]
    public async Task<UserDto> CreateUser([ActionParameter] CreateUserRequest input)
    {
        if (input.Password is null && input.PlainTextPassword is null)
            throw new("You should specify either Password or Plain text password to create a user");
        
        var securityService = new MemoqServiceFactory<ISecurityService>(
            SoapConstants.SecurityServiceUrl, Creds);

        var request = new UserInfo()
        {
            Address = input.Address,
            EmailAddress = input.EmailAddress,
            FullName = input.FullName,
            IsDisabled = input.IsDisabled ?? default,
            IsSubvendorManager = input.IsSubvendorManager ?? default,
            LTEmailAddress = input.LTEmailAddress,
            LTFullName = input.LTFullName,
            LTUsername = input.LTUsername,
            LanguagePairs = input.LanguagePairs,
            MobilePhoneNumber = input.MobilePhoneNumber,
            Password = input.Password,
            PhoneNumber = input.PhoneNumber,
            PlainTextPassword = input.PlainTextPassword,
            SecondarySID = input.SecondarySID is not null ? Guid.Parse(input.SecondarySID) : default,
            UserName = input.UserName,
        };
        var response = await securityService.Service.CreateUserAsync(request);
      
        return await GetUser(new()
        {
            UserGuid = response.ToString()
        });
    }
    
    [Action("Get user", Description = "Get user by guid")]
    public async Task<UserDto> GetUser([ActionParameter] UserRequest user)
    {
        var securityService = new MemoqServiceFactory<ISecurityService>(
            SoapConstants.SecurityServiceUrl, Creds);
            
        var response = await securityService.Service.GetUserAsync(Guid.Parse(user.UserGuid));
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