using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.MemoQ.DataSourceHandlers.EnumDataHandlers
{
    public class TaskStatusDataHandler : IStaticDataSourceHandler
    {
        Dictionary<string, string> IStaticDataSourceHandler.GetData()
        {   
            return new Dictionary<string, string>
            {
                { "0", "Invalid task" },
                { "1", "Pending" },
                { "2", "Executing" },
                { "3", "Cancelled" },
                { "4", "Completed" },
                { "5", "Failed" }
            };
        }
    }
}
