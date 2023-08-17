using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Memoq.DataSourceHandlers.Enums;

public class AnalysisAlgorithmDataHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        {"MemoQ", "MemoQ"},
        {"Trados", "Trados"},
    };
}