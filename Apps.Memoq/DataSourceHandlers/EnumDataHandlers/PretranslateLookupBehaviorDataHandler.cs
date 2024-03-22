using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class PretranslateLookupBehaviorDataHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        {"0", "Exact match with context"},
        {"1", "Exact match"},
        {"2", "Good match"},
        {"3", "Any match"},
    };
}