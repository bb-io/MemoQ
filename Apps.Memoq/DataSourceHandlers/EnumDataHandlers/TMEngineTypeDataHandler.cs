using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class TmEngineTypeDataHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        {"OldTMEngine", "Old TM engine"},
        {"NGTMEngine", "NG TM engine"},
    };
}