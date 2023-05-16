using MQS.TasksService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memoq.Models.Files.Responses
{
    public class GetAnalysisResponse : StatisticsResultForLang
    {
        public GetAnalysisResponse(StatisticsResultForLang statistics) 
        { 
            ResultData = statistics.ResultData;
            TargetLangCode = statistics.TargetLangCode;
        }
        public string Filename { get; set; }
    }
}
