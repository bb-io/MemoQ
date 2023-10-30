using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;
using MQS.Security;
using RestSharp;

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

                return new() { IsValid = response != null };
            }
            catch (Exception ex)
            {
                return new()
                {
                    IsValid = false,
                    Message = ex.InnerException?.InnerException?.Message ?? ex.Message
                };
            }
        }
    }
}
