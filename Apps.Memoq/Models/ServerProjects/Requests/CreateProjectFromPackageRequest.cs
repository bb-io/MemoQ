using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.ServerProjects.Requests;

public class CreateProjectFromPackageRequest : CreateProjectRequest
{
    [Display("File ID")]
    public string FileId { get; set; }
    
    [Display("Import resources")]
    public bool ImportResources { get; set; }
}