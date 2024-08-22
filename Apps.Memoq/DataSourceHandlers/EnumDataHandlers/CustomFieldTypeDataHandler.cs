using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.MemoQ.DataSourceHandlers.EnumDataHandlers
{
    public class CustomFieldTypeDataHandler : IStaticDataSourceHandler
    {
        protected Dictionary<string, string> EnumValues => new()
    {
        {"Free text", "FreeText"},
        {"Number", "Number"}
    };

        public Dictionary<string, string> GetData()
        {
            return EnumValues;
        }
    }
}
