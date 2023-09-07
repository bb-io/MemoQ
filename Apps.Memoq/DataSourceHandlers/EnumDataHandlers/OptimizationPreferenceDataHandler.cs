using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class OptimizationPreferenceDataHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        {"Recall", "Recall"},
        {"Mixed", "Mixed"},
        {"Fast", "Fast"},
    };
}