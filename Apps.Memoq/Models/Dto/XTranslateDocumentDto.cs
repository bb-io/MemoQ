using Blackbird.Applications.Sdk.Common;
using MQS.TasksService;

namespace Apps.Memoq.Models.Dto;

public class XTranslateDocumentDto
{
    [Display("GUID")] public string Guid { get; set; }

    [Display("Number of candidates")] public int NumberOfCandidates { get; set; }

    [Display("Number of rows")] public int NumberOfRows { get; set; }

    [Display("Number of XTranslated rows")]
    public int NumberOfXTranslatedRows { get; set; }

    public XTranslateDocumentDto(XTranslateDocumentResult doc)
    {
        Guid = doc.DocumentGuid.ToString();
        NumberOfCandidates = doc.NumberOfCandidates;
        NumberOfRows = doc.NumberOfRows;
        NumberOfXTranslatedRows = doc.NumberOfXTranslatedRows;
    }
}