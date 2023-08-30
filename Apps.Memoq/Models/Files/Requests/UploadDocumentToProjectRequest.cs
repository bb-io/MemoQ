using Apps.Memoq.DataSourceHandlers;
using Apps.Memoq.Models.ServerProjects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.Memoq.Models.Files.Requests;

public class UploadDocumentToProjectRequest : ProjectRequest
{
    public File File { get; set; }

    [Display("Target language")]
    [DataSource(typeof(TargetLanguageDataHandler))]
    public string TargetLanguageCode { get; set; }

    [Display("File name")]
    public string? FileName { get; set; }
    

}