using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Memoq.DataSourceHandlers.EnumDataHandlers;

public class ConfirmLockDataHandler : IStaticDataSourceHandler
{
    protected Dictionary<string, string> EnumValues => new()
    {
        {"0", "None"},
        {"1", "Exact match"},
        {"2", "Exact match with context"},
        {"3", "Ice spice match"},
    };

    public Dictionary<string, string> GetData()
    {
        return EnumValues;
    }
}