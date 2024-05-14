using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class XTranslateScenarioDataHandler : IStaticDataSourceHandler
{
    protected Dictionary<string, string> EnumValues => new()
    {
        {"NewRevision", "New revision"},
        {"MidProjectUpdate", "Mid project update"},
    };

    public Dictionary<string, string> GetData()
    {
        return EnumValues;
    }
}