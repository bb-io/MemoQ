using Apps.Memoq.DataSourceHandlers;
using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Apps.Memoq.Models.ServerProjects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Memoq.Models.Files.Requests;

public class UploadDocumentToProjectRequest : ProjectRequest
{
    [Display("Document")]
    public FileReference File { get; set; }

    [Display("Target languages"), StaticDataSource(typeof(TargetLanguageDataHandler))]
    public IEnumerable<string>? TargetLanguageCodes { get; set; }

    [Display("File name")]
    public string? FileName { get; set; }    

}