using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class PretranslateLookupBehaviorDataHandler : IStaticDataSourceHandler
{
    protected Dictionary<string, string> EnumValues => new()
    {
        {"0", "Exact match with context"},
        {"1", "Exact match"},
        {"2", "Good match"},
        {"3", "Any match"},
    };

    public Dictionary<string, string> GetData()
    {
        return EnumValues;
    }
}