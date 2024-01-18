using System.Net.Mime;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using MQS.TasksService;

namespace Apps.Memoq.Models.Files.Responses;

public class GetAnalysisResponse
{
    [Display("Result data")] public FileReference ResultData { get; set; }

    [Display("Target language")] public string TargetLangCode { get; set; }

    public GetAnalysisResponse(StatisticsResultForLang statistics, IFileManagementClient fileManagementClient, 
        string fileName, string? contentType = null)
    {
        using var stream = new MemoryStream(statistics.ResultData);
        ResultData = fileManagementClient.UploadAsync(stream, contentType ?? MediaTypeNames.Application.Octet, fileName)
            .Result;
        TargetLangCode = statistics.TargetLangCode;
    }
}