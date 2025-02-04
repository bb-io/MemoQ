using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using DocumentFormat.OpenXml;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class FinalTranslationStateDataHandler : IStaticDataSourceItemHandler
{
    protected Dictionary<string, string> EnumValues => new()
    {
        { "0", "No change" },
        { "1", "Confirmed" },
        { "2", "Proofread" },
        { "3", "Pretranslated" },
        { "4", "Reviewer 1 confirmed" }
    };

    public IEnumerable<DataSourceItem> GetData()
    {
        return EnumValues.Select(x => new DataSourceItem(x.Key, x.Value));
    }
}