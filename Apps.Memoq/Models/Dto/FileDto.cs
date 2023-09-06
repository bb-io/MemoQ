using Blackbird.Applications.Sdk.Common;
using MQS.ServerProject;

namespace Apps.Memoq.Models.Dto;

public class FileDto
{
    [Display("GUID")] public string Guid { get; set; }

    [Display("Parent document GUID")] public string ParentDocumentId { get; set; }

    public string Name { get; set; }

    public string Status { get; set; }

    [Display("Export path")] public string ExportPath { get; set; }

    [Display("Target language code")] public string TargetLanguageCode { get; set; }

    public FileDto(ServerProjectTranslationDocInfo2 file)
    {
        Guid = file.DocumentGuid.ToString();
        ParentDocumentId = file.ParentDocumentId.ToString();
        Name = file.DocumentName;
        Status = file.DocumentStatus.ToString();
        ExportPath = file.ExportPath;
        TargetLanguageCode = file.TargetLangCode;
    }

    public FileDto()
    {
    }
}