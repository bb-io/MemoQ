using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;
using MQS.ServerProject;

namespace Apps.MemoQ.Models.ServerProjects.Responses
{
    public class PostTranslationAnalysisRunResponse
    {
        [Display("Result status")] public ResultStatus ResultStatus { get; set; }
        [Display("Main message")] public string? MainMessage { get; set; }
        [Display("Detailed message")] public string? DetailedMessage { get; set; }
        [Display("By language")] public List<PostTranslationAnalysisLanguageDto>? ByLanguage { get; set; }
    }

    public class PostTranslationAnalysisLanguageDto
    {
        [Display("Language code")] public string? LanguageCode { get; set; }
        [Display("Summary")] public PostTranslationAnalysisItemDto? Summary { get; set; }
        [Display("Documents")] public List<PostTranslationAnalysisDocumentDto>? Documents { get; set; }
        [Display("Users")] public List<PostTranslationAnalysisUserDto>? Users { get; set; }
    }

    public class PostTranslationAnalysisDocumentDto
    {
        [Display("Document ID")] public string? DocumentId { get; set; }
        [Display("Summary")] public PostTranslationAnalysisItemDto? Summary { get; set; }
        [Display("Users")] public List<PostTranslationAnalysisUserDto>? Users { get; set; }
    }

    public class PostTranslationAnalysisUserDto : PostTranslationAnalysisItemDto
    {
        [Display("Username")] public string? Username { get; set; }
    }

    public class PostTranslationAnalysisItemDto
    {
        [Display("All")] public PostTranslationAnalysisCountsDto? All { get; set; }
        [Display("Auto propagated")] public PostTranslationAnalysisCountsDto? AutoPropagated { get; set; }
        [Display("Fragments")] public PostTranslationAnalysisCountsDto? Fragments { get; set; }
        [Display("100%")] public PostTranslationAnalysisCountsDto? Hit100 { get; set; }
        [Display("101%")] public PostTranslationAnalysisCountsDto? Hit101 { get; set; }
        [Display("50-74%")] public PostTranslationAnalysisCountsDto? Hit50_74 { get; set; }
        [Display("75-84%")] public PostTranslationAnalysisCountsDto? Hit75_84 { get; set; }
        [Display("85-94%")] public PostTranslationAnalysisCountsDto? Hit85_94 { get; set; }
        [Display("95-99%")] public PostTranslationAnalysisCountsDto? Hit95_99 { get; set; }
        [Display("No match")] public PostTranslationAnalysisCountsDto? NoMatch { get; set; }
        [Display("X-translated")] public PostTranslationAnalysisCountsDto? XTranslated { get; set; }
    }

    public class PostTranslationAnalysisCountsDto
    {
        [Display("Segments")] public int SegmentCount { get; set; }
        [Display("Source Asian chars")] public int SourceAsianCharCount { get; set; }
        [Display("Source chars")] public int SourceCharCount { get; set; }
        [Display("Source non-Asian words")] public int SourceNonAsianWordCount { get; set; }
        [Display("Source tags")] public int SourceTagCount { get; set; }
        [Display("Source words")] public int SourceWordCount { get; set; }
    }

    public class PostTranslationAnalysisReportsResponse
    {
        public IEnumerable<PostTranslationAnalysisReportInfoResponse> Reports { get; set; }

        public PostTranslationAnalysisReportsResponse(PostTranslationAnalysisReportInfo[] reports)
        {
            Reports = reports.Select(r => new PostTranslationAnalysisReportInfoResponse(r)).ToList();
        }
    }

    public class PostTranslationAnalysisReportInfoResponse
    {
        [Display("Created at")] public DateTime Created { get; set; }
        [Display("Created by")] public string CreatedBy { get; set; }
        [Display("Languages")] public IEnumerable<string> Languages { get; set; }
        [Display("Note")] public string Note { get; set; }
        [Display("Report ID")] public string ReportId { get; set; }

        public PostTranslationAnalysisReportInfoResponse(PostTranslationAnalysisReportInfo info)
        {
            Created = info.Created;
            CreatedBy = info.CreatedBy;
            Languages = info.Languages;
            Note = info.Note;
            ReportId = info.ID.ToString();
        }
    }

    public class PostTranslationAnalysisReportExportResponse
    {
        [Display("Report ID")] public string ReportId { get; set; }
        [Display("Files")] public IEnumerable<PostTranslationAnalysisExportFileDto> Files { get; set; }

        public PostTranslationAnalysisReportExportResponse(Guid reportId, IEnumerable<PostTranslationAnalysisExportFileDto> files)
        {
            ReportId = reportId.ToString();
            Files = files;
        }
    }

    public class PostTranslationAnalysisExportFileDto
    {
        [Display("Language code")] public string LanguageCode { get; set; }
        [Display("File")] public FileReference File { get; set; }

        public PostTranslationAnalysisExportFileDto(string languageCode, FileReference file)
        {
            LanguageCode = languageCode;
            File = file;
        }
    }
}
