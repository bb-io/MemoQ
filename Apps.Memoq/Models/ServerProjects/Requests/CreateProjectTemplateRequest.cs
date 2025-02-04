using Apps.Memoq.DataSourceHandlers;
using Apps.MemoQ.DataSourceHandlers;
using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.ServerProjects.Requests;

public class CreateProjectTemplateRequest
{
    [Display("Source language")]
    [DataSource(typeof(SourceLanguageDataHandler))]
    public string? SourceLangCode { get; set; }

    [Display("Target languages"), StaticDataSource(typeof(TargetLanguageDataHandler))]
    public IEnumerable<string>? TargetLangCodes { get; set; }

    [Display("Project name")]
    public string? ProjectName { get; set; }

    [Display("Template ID")]
    [DataSource(typeof(ProjectTemplateDataHandler))]
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