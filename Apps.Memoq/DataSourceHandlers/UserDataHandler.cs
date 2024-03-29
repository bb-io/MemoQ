using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using MQS.Security;

namespace Apps.Memoq.DataSourceHandlers;

public class UserDataHandler : BaseInvocable, IDataSourceHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public UserDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public Dictionary<string, string> GetData(DataSourceContext context)
    {
        var securityService = new MemoqServiceFactory<ISecurityService>(SoapConstants.SecurityServiceUrl,
            Creds);
        var users = securityService.Service.ListUsers();

        if (users is null)
        {
            return new();
        }

        return users
            .Where(x =>
            {
                if (context.SearchString is null)
                {
                    return true;
                }

                var name = x.FullName ?? x.UserName;
                return name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase);
            })
            .Take(20)
            .ToDictionary(x => x.UserGuid.ToString(), x => x.FullName ?? x.UserName);
    }
}