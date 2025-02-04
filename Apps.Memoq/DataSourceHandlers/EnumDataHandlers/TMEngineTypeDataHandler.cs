using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class TmEngineTypeDataHandler : IStaticDataSourceItemHandler
{
    protected Dictionary<string, string> EnumValues => new()
    {
        {"OldTMEngine", "Old TM engine"},
        {"NGTMEngine", "NG TM engine"},
    };

    public IEnumerable<DataSourceItem> GetData()
    {
        return EnumValues.Select(x => new DataSourceItem(x.Key, x.Value));
    }
}