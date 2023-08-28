using System.Net.Mime;
using Blackbird.Applications.Sdk.Common;
using MQS.TasksService;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.Memoq.Models.Files.Responses;

public class GetAnalysisResponse
{
    [Display("Result data")] public File ResultData { get; set; }

    [Display("Target language code")] public string TargetLangCode { get; set; }

    public GetAnalysisResponse(StatisticsResultForLang statistics, string fileName, string? contentType = null)
    {
        ResultData = new(statistics.ResultData)
        {
            Name = fileName,
            ContentType = contentType ?? MediaTypeNames.Application.Octet
        };
        TargetLangCode = statistics.TargetLangCode;
    }
}