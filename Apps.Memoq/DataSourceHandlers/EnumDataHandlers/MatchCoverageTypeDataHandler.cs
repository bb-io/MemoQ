using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class MatchCoverageTypeDataHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData()
    {
        return new Dictionary<string, string>
        {
            {"100", "Full single hit"},
            {"200", "Full multiple hits"},
            {"300", "Not full"}
        };
    }
}