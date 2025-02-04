using Apps.Memoq.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.Files.Requests;

public class GetDocumentRequest
{
    [Display("Project ID"), DataSource(typeof(ProjectDataHandler))]
    public string ProjectGuid { get; set; }
    
    [Display("File ID"), DataSource(typeof(DocumentDataHandler))]
    public string DocumentGuid { get; set; }
}