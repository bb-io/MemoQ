using Blackbird.Applications.Sdk.Common;
using MQS.ServerProject;

namespace Apps.Memoq.Models.Dto;

public class FileDto
{
    [Display("File ID")] public string Guid { get; set; }

    [Display("Parent file ID")] public string ParentDocumentId { get; set; }

    public string Name { get; set; }
    
    [Display("File extension")]
    public string FileExtension { get; set; }

    public string Status { get; set; }

    [Display("Export path")] public string ExportPath { get; set; }

    [Display("Target language code")] public string TargetLanguageCode { get; set; }

    public FileDto(ServerProjectTranslationDocInfo2 file)
    {
        Guid = file.DocumentGuid.ToString();
        ParentDocumentId = file.ParentDocumentId.ToString();
        Name = Path.GetFileNameWithoutExtension(file.DocumentName);
        FileExtension = Path.GetExtension(file.DocumentName);
        Status = file.DocumentStatus.ToString();
        ExportPath = file.ExportPath;
        TargetLanguageCode = file.TargetLangCode;
    }

    public FileDto(FileDto file)
    {
        Guid = file.Guid;
        ParentDocumentId = file.ParentDocumentId;
        Name = file.Name;
        FileExtension = file.FileExtension;
        Status = file.Status;
        ExportPath = file.ExportPath;
        TargetLanguageCode = file.TargetLanguageCode;
    }
}