using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class TmEngineTypeDataHandler : IStaticDataSourceHandler
{
    protected Dictionary<string, string> EnumValues => new()
    {
        {"OldTMEngine", "Old TM engine"},
        {"NGTMEngine", "NG TM engine"},
    };

    public Dictionary<string, string> GetData()
    {
        return EnumValues;
    }
}