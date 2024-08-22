using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.MemoQ.DataSourceHandlers.EnumDataHandlers
{
    public class CustomFieldTypeDataHandler : IStaticDataSourceHandler
    {
        protected Dictionary<string, string> EnumValues => new()
    {
        {"FreeText", "Free text"},
        {"Number", "Number"}
    };

        public Dictionary<string, string> GetData()
        {
            return EnumValues;
        }
    }
}
