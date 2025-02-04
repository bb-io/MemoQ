using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.MemoQ;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using MQS.Security;

namespace Apps.Memoq.DataSourceHandlers;

public class UserDataHandler : MemoqInvocable, IDataSourceHandler
{
    public UserDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public Dictionary<string, string> GetData(DataSourceContext context)
    {
        var users = SecurityService.Service.ListUsers();

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