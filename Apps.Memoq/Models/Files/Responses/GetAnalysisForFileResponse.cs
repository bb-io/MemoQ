using MQS.ServerProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memoq.Models.Files.Responses
{
    public class GetAnalysisForFileResponse
    {
        public List<AnalysisAsCSVResultForLang> Analyses { get; set; }
    }
}
