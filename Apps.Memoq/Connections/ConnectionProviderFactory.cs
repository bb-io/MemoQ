using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Connections;

public class ConnectionProviderFactory : IConnectionProviderFactory
{
    public IEnumerable<IConnectionProvider> Create()
    {
        yield return new ConnectionProvider();
    }
}

