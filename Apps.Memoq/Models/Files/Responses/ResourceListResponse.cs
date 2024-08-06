using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.MemoQ.Models.Files.Responses
{
    public class ResourceListResponse
    {
        [Display("Resource IDs")]
        public IEnumerable<string> ResourceIds { get; set; }
    }
}
