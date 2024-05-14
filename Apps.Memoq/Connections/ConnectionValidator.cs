using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;
using MQS.Security;

namespace Apps.Memoq.Connections
{
    public class ConnectionValidator : IConnectionValidator
    {
        public async ValueTask<ConnectionValidationResponse> ValidateConnection(
            IEnumerable<AuthenticationCredentialsProvider> authProviders, CancellationToken cancellationToken)
        {
            try
            {
                var securityService = new MemoqServiceFactory<ISecurityService>(SoapConstants.SecurityServiceUrl, authProviders);

                var response = securityService.Service.ListUsers();
                return new() { IsValid = response != null, Message = response == null ? "No response from server" : null };
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Exception: {ex.Message}; Exception type: {ex.GetType()}; InnerException: {ex.InnerException?.Message}; InnerException.InnerException: {ex.InnerException?.InnerException?.Message}";
                return new()
                {
                    IsValid = false,
                    Message = exceptionMessage
                };
            }
        }
    }
}
