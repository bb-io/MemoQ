using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memoq.Models.ServerProjects.Requests
{
    public class DownloadXliffRequest : ProjectRequest
    {
        [Display("Document GUID")]
        public string DocumentGuid { get; set; }

        [Display("Include full version history?")]
        public bool? FullVersionHistory { get; set; }

        [Display("Include skeleton?")]
        public bool? IncludeSkeleton { get; set; }

        [Display("Save compressed?")]
        public bool? SaveCompressed { get; set; }
    }
}
