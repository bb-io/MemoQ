using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class ResourceTypeDataHandler : IStaticDataSourceHandler
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

    public Dictionary<string, string> GetData()
    {
        return EnumValues;
    }
}