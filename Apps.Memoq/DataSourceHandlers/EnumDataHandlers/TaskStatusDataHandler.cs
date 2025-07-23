using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.MemoQ.DataSourceHandlers.EnumDataHandlers
{
    public class TaskStatusDataHandler : IStaticDataSourceHandler
    {
        Dictionary<string, string> IStaticDataSourceHandler.GetData()
        {   
            return new Dictionary<string, string>
            {
                { "Invalid task", "Invalid task" },
                { "Pending", "Pending" },
                { "Executing", "Executing" },
                { "Cancelled", "Cancelled" },
                { "Completed", "Completed" },
                { "Failed", "Failed" }
            };
        }
    }
}
