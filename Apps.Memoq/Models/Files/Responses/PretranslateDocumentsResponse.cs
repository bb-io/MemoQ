using Blackbird.Applications.Sdk.Common;
using MQS.ServerProject;

namespace Apps.Memoq.Models.Files.Responses;

public class PretranslateDocumentsResponse
{
    [Display("Result status")]
    public string ResultStatus { get; set; }
    
    [Display("Main message")]
    public string MainMessage { get; set; }
    
    [Display("Detailed message")]
    public string DetailedMessage { get; set; }

    public PretranslateDocumentsResponse(ResultInfo resultInfo)
    {
        ResultStatus = resultInfo.ResultStatus.ToString();
        MainMessage = resultInfo.MainMessage;
        DetailedMessage = resultInfo.DetailedMessage;
    }
}