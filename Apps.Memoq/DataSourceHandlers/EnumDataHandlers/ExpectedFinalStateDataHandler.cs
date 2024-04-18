using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class ExpectedFinalStateDataHandler : IStaticDataSourceHandler
{
    protected Dictionary<string, string> EnumValues => new()
    {
        {"SameAsPrevious", "Same as previous"},
        {"Pretranslated", "Pretranslated"},
        {"Confirmed", "Confirmed"},
        {"Proofread", "Proofread"},
        {"Reviewer1Confirmed", "Reviewer 1 confirmed"},
    };

    public Dictionary<string, string> GetData()
    {
        return EnumValues;
    }
}