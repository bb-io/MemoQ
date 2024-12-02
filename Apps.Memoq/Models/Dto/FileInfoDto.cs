using Blackbird.Applications.Sdk.Common;
using MQS.ServerProject;

namespace Apps.MemoQ.Models.Dto
{
    public class FileInfoDto
    {
        [Display("File GUID")] public string Guid { get; set; }

        [Display("Parent document GUID")] public string ParentDocumentId { get; set; }

        public string Name { get; set; }

        [Display("File extension")]
        public string FileExtension { get; set; }

        public string Status { get; set; }

        [Display("Import path")] public string ImportPath { get; set; }
        [Display("Export path")] public string ExportPath { get; set; }

        [Display("Target language code")] public string TargetLanguageCode { get; set; }

        [Display("Confirmed segment count")]
        public int ConfirmedSegmentCount { get; set; }

        [Display("Confirmed word count")]
        public int ConfirmedWordCount { get; set; }

        [Display("External document ID")]
        public string ExternalDocumentId { get; set; }

        [Display("Is image?")]
        public bool IsImage { get; set; }

        [Display("Locked character count")]
        public int LockedCharacterCount { get; set; }

        [Display("Locked segment count")]
        public int LockedSegmentCount { get; set; }

        [Display("Locked word count")]
        public int LockedWordCount { get; set; }

        [Display("Web translation URL")]
        public string WebTransUrl { get; set; }

        [Display("Total character count")]
        public int TotalCharacterCount { get; set; }

        [Display("Total segment count")]
        public int TotalSegmentCount { get; set; }

        [Display("Total word count")]
        public int TotalWordCount { get; set; }

        public FileInfoDto(ServerProjectTranslationDocInfo2 file)
        {
            Guid = file.DocumentGuid.ToString();
            ParentDocumentId = file.ParentDocumentId.ToString();
            Name = Path.GetFileNameWithoutExtension(file.DocumentName);
            FileExtension = Path.GetExtension(file.DocumentName);
            Status = file.DocumentStatus.ToString();
            ImportPath = file.ImportPath;
            ExportPath = file.ExportPath;
            TargetLanguageCode = file.TargetLangCode;
            ConfirmedSegmentCount = file.ConfirmedSegmentCount;
            ConfirmedWordCount = file.ConfirmedWordCount;
            ExternalDocumentId = file.ExternalDocumentId;
            IsImage = file.IsImage;
            LockedCharacterCount = file.LockedCharacterCount;
            LockedSegmentCount = file.LockedSegmentCount;
            LockedWordCount = file.LockedWordCount;
            WebTransUrl = file.WebTransUrl;
            TotalCharacterCount = file.TotalCharacterCount;
            TotalSegmentCount = file.TotalSegmentCount;
            TotalWordCount = file.TotalWordCount;
        }

        public FileInfoDto(FileInfoDto file)
        {
            Guid = file.Guid;
            ParentDocumentId = file.ParentDocumentId;
            Name = file.Name;
            FileExtension = file.FileExtension;
            Status = file.Status;
            ExportPath = file.ExportPath;
            ImportPath = file.ImportPath;
            ExportPath = file.ExportPath;
            TargetLanguageCode = file.TargetLanguageCode;
            ConfirmedSegmentCount = file.ConfirmedSegmentCount;
            ConfirmedWordCount = file.ConfirmedWordCount;
            ExternalDocumentId = file.ExternalDocumentId;
            IsImage = file.IsImage;
            LockedCharacterCount = file.LockedCharacterCount;
            LockedSegmentCount = file.LockedSegmentCount;
            LockedWordCount = file.LockedWordCount;
            WebTransUrl = file.WebTransUrl;
            TotalCharacterCount = file.TotalCharacterCount;
            TotalSegmentCount = file.TotalSegmentCount;
            TotalWordCount = file.TotalWordCount;
        }
    }
}
