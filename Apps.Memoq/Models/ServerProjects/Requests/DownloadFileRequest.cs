using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memoq.Models.ServerProjects.Requests
{
    public class DownloadFileRequest
    {
        public string ProjectGuid { get; set; }
        public string FileGuid { get; set; }
    }
}
