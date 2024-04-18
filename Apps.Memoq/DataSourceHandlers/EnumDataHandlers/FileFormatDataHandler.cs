using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class FileFormatDataHandler : IStaticDataSourceHandler
{
    protected Dictionary<string, string> EnumValues => new()
    {
        {"Html", "HTML"},
        {"CSV_WithTable", "CSV with table"},
        {"CSV_Trados", "Trados CSV"},
        {"CSV_MemoQ", "MemoQ CSV"},
    };

    public Dictionary<string, string> GetData()
    {
        return EnumValues;
    }
}