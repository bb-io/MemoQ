using Apps.Memoq.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.ServerProjects.Requests;

public class CreateProjectTemplateRequest
{
    [Display("Source language")]
    [DataSource(typeof(SourceLanguageDataHandler))]
    public string SourceLangCode { get; set; }

    [Display("Target language")]
    [DataSource(typeof(TargetLanguageDataHandler))]
    public string TargetLangCode { get; set; }

    [Display("Project name")]
    public string ProjectName { get; set; }

    [Display("Template GUID")]
    public string TemplateGuid { get; set; }
    
    [Display("Client")]
    public string? Client { get; set; }

    [Display("Description")]
    public string? Description { get; set; }

    [Display("Domain")]
    public string? Domain { get; set; }

    [Display("Subject")]
    public string? Subject { get; set; }
}