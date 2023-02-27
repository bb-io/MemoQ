using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;

namespace Apps.Memoq.Connections;

public class ConnectionProvider : IConnectionProvider
{
    public AuthenticationCredentialsProvider Create(IDictionary<string, string> connectionValues)
    {
        var credential = connectionValues.First(x => x.Key == "apiKey");
        return new AuthenticationCredentialsProvider(AuthenticationCredentialsRequestLocation.None, credential.Key, credential.Value);
    }

    public string ConnectionName  =>  "test";

    
    public IEnumerable<string> ConnectionProperties  => new [] {"url", "apiKey" };
}