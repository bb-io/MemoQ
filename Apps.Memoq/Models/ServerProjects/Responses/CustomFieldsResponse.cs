using Apps.MemoQ.Models.Dto;
using Blackbird.Applications.Sdk.Common;

namespace Apps.MemoQ.Models.ServerProjects.Responses
{
    public class CustomFieldsResponse
    {
        [Display("Custom fields")]
        public List<CustomFieldDto> CustomFields { get; set; }
    }
}
