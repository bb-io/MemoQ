using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.MemoQ.DataSourceHandlers.EnumDataHandlers
{
    public class YesNoDataHandler : IStaticDataSourceItemHandler
    {
        protected Dictionary<string, string> EnumValues => new()
        {
            {"true", "Yes"},
            {"false", "No"}
        };

        public IEnumerable<DataSourceItem> GetData()
        {
            return EnumValues.Select(item => new DataSourceItem
            {
                Value = item.Key,
                DisplayName = item.Value
            });
        }
    }
}
