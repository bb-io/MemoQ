using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Memoq.DataSourceHandlers.Enums;

public class XTranslateScenarioDataHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        {"NewRevision", "New revision"},
        {"MidProjectUpdate", "Mid project update"},
    };
}