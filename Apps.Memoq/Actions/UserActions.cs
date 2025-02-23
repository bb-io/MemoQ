using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.Memoq.Models.Dto;
using Apps.Memoq.Models.ServerProjects.Requests;
using Apps.Memoq.Models.Users.Requests;
using Apps.Memoq.Models.Users.Responses;
using Apps.MemoQ;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using MQS.Security;
using MQS.ServerProject;
using UserInfo = MQS.Security.UserInfo;

namespace Apps.Memoq.Actions;

[ActionList]
public class UserActions : MemoqInvocable
{
    public UserActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("Create user", Description = "Create a new user")]
    public async Task<UserDto> CreateUser([ActionParameter] CreateUserRequest input)
    {
        if (input.Password is null && input.PlainTextPassword is null)
            throw new PluginMisconfigurationException("You should specify either Password or Plain text password to create a user");


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
            SecondarySID = input.SecondarySID is not null ? GuidExtensions.ParseWithErrorHandling(input.SecondarySID) : default,
            UserName = input.UserName,
        };
        var response = await ExecuteWithHandling(() => SecurityService.Service.CreateUserAsync(request));

        return await GetUser(new()
        {
            UserGuid = response.ToString()
        });
    }

    [Action("Get user", Description = "Get user information")]
    public async Task<UserDto> GetUser([ActionParameter] UserRequest user)
    {
        var response = await ExecuteWithHandling(() => SecurityService.Service.GetUserAsync(GuidExtensions.ParseWithErrorHandling(user.UserGuid)));
        return new(response);
    }

    [Action("Delete user", Description = "Delete user")]
    public async Task DeleteUser([ActionParameter] UserRequest user)
    {
        await ExecuteWithHandling(() => SecurityService.Service.DeleteUserAsync(GuidExtensions.ParseWithErrorHandling(user.UserGuid)));
    }

    [Action("Add users to project", Description = "Add users to project")]
    public async Task AddUserToProject([ActionParameter] UsersRequest request,
        [ActionParameter] ProjectRequest projectRequest)
    {
        var users = request.UserGuids.Select(x => new ServerProjectUserInfo
        {
            UserGuid = GuidExtensions.ParseWithErrorHandling(x), PermForLicense = true, ProjectRoles = new ServerProjectRoles(),
        }).ToArray();
        await ExecuteWithHandling(() => ProjectService.Service.SetProjectUsersAsync(GuidExtensions.ParseWithErrorHandling(projectRequest.ProjectGuid), users));
    }
}