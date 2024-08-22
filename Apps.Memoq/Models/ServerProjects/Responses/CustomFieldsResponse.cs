using Apps.MemoQ.Models.Dto;
using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.MemoQ.Models.ServerProjects.Responses
{
    public class CustomFieldsResponse
    {
        [Display("Custom fields")]
        public List<CustomFieldDto> CustomFields { get; set; }
    }
}
