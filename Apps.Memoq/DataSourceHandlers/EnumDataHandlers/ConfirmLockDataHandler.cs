using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class ConfirmLockDataHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        {"0", "None"},
        {"1", "Exact match"},
        {"2", "Exact match with context"},
        {"3", "Ice spice match"},
    };
}