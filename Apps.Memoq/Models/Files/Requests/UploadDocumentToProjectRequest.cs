using Apps.Memoq.Models.ServerProjects.Requests;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.Files.Requests;

public class UploadDocumentToProjectRequest : ProjectRequest
{
    public byte[] File { get; set; }
    
    [Display("File name")]
    public string FileName { get; set; }
    
    [Display("Target language codes")]
    public IEnumerable<string>? TargetLanguageCodes { get; set; }
}