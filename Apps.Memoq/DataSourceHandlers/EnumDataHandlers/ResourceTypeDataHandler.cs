using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class ResourceTypeDataHandler : IStaticDataSourceItemHandler
{
    protected Dictionary<string, string> EnumValues => new()
    {
        { "0", "Auto translate" },
        { "1", "Non translate" },
        { "3", "Ignore lists" },
        { "4", "Segmentation rules" },
        { "5", "TM settings" },
        { "8", "Path rules" },
        { "9", "QA settings" },
        { "10", "Stopwords" },
        { "11", "LQA" },
        { "12", "LiveDocs settings" },
        { "13", "Web search settings" },
        { "14", "Font substitution" },
        { "15", "Keyboard shortcuts 2" },
        { "16", "Project template" },
        { "17", "MT settings" }
    };

    public IEnumerable<DataSourceItem> GetData()
    {
        return EnumValues.Select(x => new DataSourceItem(x.Key, x.Value));
    }
}