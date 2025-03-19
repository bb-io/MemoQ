using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.MemoQ.Models.ServerProjects.Responses
{
    public class EditDistanceReportExportResponse
    {
        [Display("Report ID")] 
        public string ReportId { get; set; }

        [Display("File")]
        public FileReference File { get; set; }

        public EditDistanceReportExportResponse(Guid reportId, FileReference file)
        {
            ReportId = reportId.ToString();
            File = file;
        }
    }
}
