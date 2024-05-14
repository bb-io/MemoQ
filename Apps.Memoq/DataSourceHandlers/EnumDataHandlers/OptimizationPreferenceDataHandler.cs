using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class OptimizationPreferenceDataHandler : IStaticDataSourceHandler
{
    protected Dictionary<string, string> EnumValues => new()
    {
        {"Recall", "Recall"},
        {"Mixed", "Mixed"},
        {"Fast", "Fast"},
    };

    public Dictionary<string, string> GetData()
    {
        return EnumValues;
    }
}