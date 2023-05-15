using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memoq.Models.Files.Requests
{
    public class GetAnalysisForFileRequest
    {
        public string ProjectGuid { get; set; }

        public string DocumentGuid { get; set; }
    }
}
