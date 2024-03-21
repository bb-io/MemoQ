using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class ResourceTypeDataHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        { "0", "Auto translate" },
        { "1", "Non translate" },
        { "2", "Auto correct" },
        { "3", "Ignore lists" },
        { "4", "Segmentation rules" },
        { "5", "TM settings" },
        { "6", "Filter configurations" },
        { "7", "Keyboard shortcuts" },
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
}