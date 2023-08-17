using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Memoq.DataSourceHandlers.Enums;

public class TMEngineTypeDataHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        {"OldTMEngine", "Old TM engine"},
        {"NGTMEngine", "NG TM engine"},
    };
}