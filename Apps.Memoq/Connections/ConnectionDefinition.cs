using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memoq.Connections
{
    public class ConnectionDefinition : IConnectionDefinition
    {
        public IEnumerable<ConnectionPropertyGroup> ConnectionPropertyGroups => new List<ConnectionPropertyGroup>()
        {
            new ConnectionPropertyGroup
            {
                Name = "Developer API token",
                AuthenticationType = ConnectionAuthenticationType.Undefined,
                ConnectionUsage = ConnectionUsage.Actions,
                ConnectionProperties = new List<ConnectionProperty>()
                {
                    new ConnectionProperty("url"),
                    new ConnectionProperty("apiKey")
                }
            }
        };

        public IEnumerable<AuthenticationCredentialsProvider> CreateAuthorizationCredentialsProviders(Dictionary<string, string> values)
        {
            var url = values.First(v => v.Key == "url");
            yield return new AuthenticationCredentialsProvider(
                AuthenticationCredentialsRequestLocation.None,
                url.Key,
                url.Value
            );

            var apiKey = values.First(v => v.Key == "apiKey");
            yield return new AuthenticationCredentialsProvider(
                AuthenticationCredentialsRequestLocation.None,
                apiKey.Key,
                apiKey.Value
            );
        }
    }
}
