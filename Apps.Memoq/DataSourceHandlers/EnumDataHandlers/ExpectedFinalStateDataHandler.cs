using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class ExpectedFinalStateDataHandler : IStaticDataSourceItemHandler
{
    protected Dictionary<string, string> EnumValues => new()
    {
        {"SameAsPrevious", "Same as previous"},
        {"Pretranslated", "Pretranslated"},
        {"Confirmed", "Confirmed"},
        {"Proofread", "Proofread"},
        {"Reviewer1Confirmed", "Reviewer 1 confirmed"},
    };

    public IEnumerable<DataSourceItem> GetData()
    {
        return EnumValues.Select(x => new DataSourceItem(x.Key, x.Value));
    }
}