using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;

public class ProjectStatusDataHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData()
    {
        return new()
        {
            ["Live"] = "Live",
            ["WrappedUp"] = "Wrapped up",
        };
    }
}