using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class MatchCoverageTypeDataHandler : IStaticDataSourceItemHandler
{
    protected Dictionary<string, string> EnumValues => new()
    {
        {"100", "Full single hit"},
        {"200", "Full multiple hits"},
        {"300", "Not full"}
    };

    public IEnumerable<DataSourceItem> GetData()
    {
        return EnumValues.Select(x => new DataSourceItem(x.Key, x.Value));
    }
}