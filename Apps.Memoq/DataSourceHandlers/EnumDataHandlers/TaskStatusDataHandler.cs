using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.MemoQ.DataSourceHandlers.EnumDataHandlers
{
    public class TaskStatusDataHandler : IStaticDataSourceHandler
    {
        Dictionary<string, string> IStaticDataSourceHandler.GetData()
        {
            return new Dictionary<string, string>
            {
                { "Invalid task", "0" },
                { "Pending", "1" },
                { "Executing", "2" },
                { "Cancelled", "3" },
                { "Completed", "4" },
                { "Failed", "5" }
            };
        }
    }
}
