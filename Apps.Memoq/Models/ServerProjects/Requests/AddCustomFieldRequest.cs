using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.MemoQ.Models.ServerProjects.Requests
{
    public class AddCustomFieldRequest
    {
        [Display("Field name")]
        public string Name { get; set; }

        [StaticDataSource(typeof(CustomFieldTypeDataHandler))]
        [Display("Field type")]
        public string Type { get; set; }

        public string Value { get; set; }
    }
}
