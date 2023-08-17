using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Memoq.DataSourceHandlers.Enums;

public class FileFormatDataHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        {"Html", "HTML"},
        {"CSV_WithTable", "CSV with table"},
        {"CSV_Trados", "Trados CSV"},
        {"CSV_MemoQ", "MemoQ CSV"},
    };
}