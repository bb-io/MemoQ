using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class AnalysisAlgorithmDataHandler : IStaticDataSourceHandler
{
    protected Dictionary<string, string> EnumValues => new()
    {
        {"MemoQ", "MemoQ"},
        {"Trados", "Trados"},
    };

    public Dictionary<string, string> GetData()
    {
        return EnumValues;
    }
}