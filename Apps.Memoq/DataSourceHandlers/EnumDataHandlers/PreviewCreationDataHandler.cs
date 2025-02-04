using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.MemoQ.DataSourceHandlers.EnumDataHandlers
{
    public class PreviewCreationDataHandler : IStaticDataSourceItemHandler
    {
        protected Dictionary<string, string> EnumValues => new()
        {
            {"2", "Create preview"},
            {"1", "Disable preview creation"},
            {"0", "Determined by project settings" }
        };

        public IEnumerable<DataSourceItem> GetData()
        {
            return EnumValues.Select(x => new DataSourceItem(x.Key, x.Value));
        }
    }
}
