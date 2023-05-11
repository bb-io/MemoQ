using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.Memoq.Models.Users.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using MQS.Security;

namespace Apps.Memoq.Actions
{
    [ActionList]
    public class UserActions
    {
        [Action("List all users", Description = "List all users")]
        public ListAllUsersResponse ListAllUsers(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {
            var securityService = new MemoqServiceFactory<ISecurityService>(ApplicationConstants.SecurityServiceUrl, authenticationCredentialsProviders);
            return new ListAllUsersResponse()
            {
                Users = securityService.Service.ListUsers()
            };
        }

        [Action("Get user", Description = "Get user by guid")]
        public UserInfo GetUser(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] string userGuid)
        {
            var securityService = new MemoqServiceFactory<ISecurityService>(ApplicationConstants.SecurityServiceUrl, authenticationCredentialsProviders);
            return securityService.Service.GetUser(Guid.Parse(userGuid));
        }

        [Action("Delete user", Description = "Delete user by guid")]
        public void DeleteUser(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] string userGuid)
        {
            var securityService = new MemoqServiceFactory<ISecurityService>(ApplicationConstants.SecurityServiceUrl, authenticationCredentialsProviders);
            securityService.Service.DeleteUser(Guid.Parse(userGuid));
        }
    }
}
