using Apps.MemoQ.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.MemoQ.Models.ServerProjects.Requests
{
    public class GetCustomFieldRequest
    {
        [Display("Field name")]
        [DataSource(typeof(CustomFieldDataHandler))]
        public string Field { get; set; }
    }
}
