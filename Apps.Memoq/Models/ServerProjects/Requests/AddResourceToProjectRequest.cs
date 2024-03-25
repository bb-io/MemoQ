using Apps.Memoq.DataSourceHandlers;
using Apps.Memoq.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.ServerProjects.Requests;

// Docs about ObjectIds: https://docs.memoq.com/current/api-docs/wsapi/api/serverprojectservice/MemoQServices.ServerProjectResourceAssignmentForResourceType.html
public class AddResourceToProjectRequest
{
    [Display("Resource type"), DataSource(typeof(ResourceTypeDataHandler))]
    public string ResourceType { get; set; }
    
    [Display("Resources"), DataSource(typeof(ResourceDataHandler))]
    public IEnumerable<string> ResourceGuids { get; set; }

    [Display("Object ids", Description = "If you are not sure about the object ids, you can use other optional inputs and leave this empty")]
    public IEnumerable<string>? ObjectIds { get; set; }

    [Display("Source languages", Description = "Can be used for SegRules resource type. This values will be pushed to Object ids"), DataSource(typeof(SourceLanguageDataHandler))]
    public IEnumerable<string>? SourceLanguages { get; set; }

    [Display("Target languages", Description = "Should be used for AutoTrans, IgnoreLists and SegRules resource types"), DataSource(typeof(TargetLanguageDataHandler))]
    public IEnumerable<string>? TargetLanguages { get; set; }

    [Display("Translation memories", Description = "Can be used for TMSettings resource type"), DataSource(typeof(TranslationMemoryDataHandler))]
    public IEnumerable<string>? Tms { get; set; }
}