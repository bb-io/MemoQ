using Blackbird.Applications.Sdk.Common;
using MQS.TasksService;

namespace Apps.Memoq.Models.Files.Responses
{
    public class GetAnalysisResponse
    {
        [Display("Result data")] public byte[] ResultData { get; set; }

        [Display("Target language code")] public string TargetLangCode { get; set; }

        [Display("File name")] public string Filename { get; set; }

        public GetAnalysisResponse(StatisticsResultForLang statistics)
        {
            ResultData = statistics.ResultData;
            TargetLangCode = statistics.TargetLangCode;
        }
    }
}