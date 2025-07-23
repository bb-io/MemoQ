using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.MemoQ.DataSourceHandlers.EnumDataHandlers
{
    public class TaskStatusDataHandler : IStaticDataSourceHandler
    {
        Dictionary<string, string> IStaticDataSourceHandler.GetData()
        {
            return new Dictionary<string, string>
            {
                { "Pending", "Pending" },
                { "Executing", "Executing" },
                { "Completed", "Completed" },
                { "Failed", "Failed" },
                { "Cancelled", "Cancelled" }
            };
        }
    }
}
