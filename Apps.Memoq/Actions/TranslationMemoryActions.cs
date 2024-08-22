using System.Text;
using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.Memoq.Models.Dto;
using Apps.Memoq.Models.ServerProjects.Requests;
using Apps.Memoq.Models.TranslationMemories.Requests;
using Apps.Memoq.Models.TranslationMemories.Responses;
using Apps.Memoq.Utils.FileUploader;
using Apps.Memoq.Utils.FileUploader.Managers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.Sdk.Utils.Parsers;
using MQS.ServerProject;
using MQS.TM;
using TMEngineType = MQS.TM.TMEngineType;
using TMOptimizationPreference = MQS.TM.TMOptimizationPreference;

namespace Apps.Memoq.Actions;

[ActionList]
public class TranslationMemoryActions : BaseInvocable
{
    private readonly IFileManagementClient _fileManagementClient;
    
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public TranslationMemoryActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) 
        : base(invocationContext)
    {
        _fileManagementClient = fileManagementClient;
    }

    [Action("List translation memories", Description = "List translation memories")]
    public ListTranslationMemoriesResponse ListTranslationMemories(
        [ActionParameter] LanguagesRequest input)
    {
        using var tmService = new MemoqServiceFactory<ITMService>(
            SoapConstants.TranslationMemoryServiceUrl, Creds);

        var response = tmService.Service.ListTMs(input.SourceLanguage, input.TargetLanguage);
        var translationMemories = response.Select(x => new TmDto(x)).ToArray();
            
        return new()
        {
            TranslationMemories = translationMemories
        };
    }

    [Action("Create translation memory", Description = "Create translation memory")]
    public TmDto CreateTranslationMemory([ActionParameter] CreateTranslationMemoryRequest input)
    {
        using var tmService = new MemoqServiceFactory<ITMService>(
            SoapConstants.TranslationMemoryServiceUrl, Creds);

        var tmGuid = tmService.Service.CreateAndPublish(new()
        {
            Name = input.Name,
            SourceLanguageCode = input.SourceLanguage,
            TargetLanguageCode = input.TargetLanguage,
            Client = input.Client,
            Domain = input.Domain,
            Subject = input.Subject,
            Project = input.Project,
            AllowMultiple = input.AllowMultiple ?? default,
            AllowReverseLookup = input.AllowReverseLookup ?? default,
            CreatorUsername = input.CreatorUsername,
            OptimizationPreference = EnumParser.Parse<TMOptimizationPreference>(input.OptimizationPreference,
                nameof(input.OptimizationPreference)) ?? default,
            StoreDocumentFullPath = input.StoreDocumentFullPath ?? default,
            StoreDocumentName = input.StoreDocumentName ?? default,
            StoreFormatting = input.StoreFormatting ?? default,
            TMEngineType =
                EnumParser.Parse<TMEngineType>(input.TmEngineType, nameof(input.TmEngineType)) ?? default,
            UseContext = input.UseContext ?? default,
            UseIceSpiceContext = input.UseIceSpiceContext ?? default,
        });

        var response = tmService.Service.GetTMInfo(tmGuid);
        return new(response);
    }
    
    [Action("Get translation memory", Description = "Get details of a specific translation memory")]
    public async Task<TmDto> GetTranslationMemory([ActionParameter] TranslationMemoryRequest input)
    {
        using var tmService = new MemoqServiceFactory<ITMService>(
            SoapConstants.TranslationMemoryServiceUrl, Creds);

        var response = await tmService.Service.GetTMInfoAsync(Guid.Parse(input.TmGuid));
        return new(response);
    }   
    
    [Action("Delete translation memory", Description = "Delete specific translation memory")]
    public async Task DeleteTranslationMemory([ActionParameter] TranslationMemoryRequest input)
    {
        using var tmService = new MemoqServiceFactory<ITMService>(
            SoapConstants.TranslationMemoryServiceUrl, Creds);

        await tmService.Service.DeleteTMAsync(Guid.Parse(input.TmGuid));
    }    
    
    [Action("Update translation memory properties", Description = "Update specific translation memory properties")]
    public async Task UpdateTranslationMemoryProperties(
        [ActionParameter] TranslationMemoryRequest tm,
        [ActionParameter] UpdateTranslationMemoryPropertiesRequest input)
    {
        using var tmService = new MemoqServiceFactory<ITMService>(
            SoapConstants.TranslationMemoryServiceUrl, Creds);

        await tmService.Service.UpdatePropertiesAsync(new()
        {
            Guid = Guid.Parse(tm.TmGuid),
            Name = input.Name,
            Client = input.Client,
            Domain = input.Domain,
            Subject = input.Subject,
            Project = input.Project,
            OptimizationPreference = EnumParser.Parse<TMOptimizationPreference>(input.OptimizationPreference,
                nameof(input.OptimizationPreference)) ?? default,
            StoreDocumentFullPath = input.StoreDocumentFullPath ?? default,
            StoreDocumentName = input.StoreDocumentName ?? default,
        });
    }

    [Action("Import TMX file", Description = "Import TMX file")]
    public async Task<ImportTmxFileResponse> ImportTmxFile([ActionParameter] ImportTmxFileRequest input)
    {
        using var tmService = new MemoqServiceFactory<ITMService>(
            SoapConstants.TranslationMemoryServiceUrl, Creds);

        var manager = new TmxUploadManager(tmService.Service);
        var file = await _fileManagementClient.DownloadAsync(input.File);
        var fileBytes = await file.GetByteData();
        var result = FileUploader.UploadFile(fileBytes, manager, input.TmGuid);

        return new()
        {
            Guid = result.ToString()
        };
    }
    
    [Action("Import translation memory scheme from XML", Description = "Import specific translation memory's scheme from XML")]
    public async Task<ImportTmSchemeFromXmlResponse> ImportTmSchemeFromXml([ActionParameter] ImportTmSchemeRequest input)
    {
        using var tmService = new MemoqServiceFactory<ITMService>(
            SoapConstants.TranslationMemoryServiceUrl, Creds);

        var file = await _fileManagementClient.DownloadAsync(input.File);
        var fileBytes = await file.GetByteData();
        var xml = Encoding.UTF8.GetString(fileBytes);
        var response = await tmService.Service
            .ImportTMMetadataSchemeFromXMLAsync(Guid.Parse(input.TmGuid), xml);

        return new()
        {
            ConflictedFields = response.ConflictedFields
        };
    }
    
    [Action("Add translation memory to project", Description = "Add translation memory to project by UId")]
    public async Task AddTranslationMemoryToProject(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] AddTranslationMemoryToProjectRequest translationMemoryRequest)
    {
        var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);
        
        
        Guid masterGuid = translationMemoryRequest.MasterTmGuid != null
            ? Guid.Parse(translationMemoryRequest.MasterTmGuid)
            : Guid.Empty;
        
        Guid primaryGuid = translationMemoryRequest.PrimaryTmGuid != null
            ? Guid.Parse(translationMemoryRequest.PrimaryTmGuid)
            : Guid.Empty;
        
        var tmGuids = translationMemoryRequest.TmGuids ?? new List<string>();

        var tmAssignments = new ServerProjectTMAssignmentsForTargetLang
        {
            TMGuids = tmGuids.Select(Guid.Parse).ToArray(),
            TargetLangCode = translationMemoryRequest.TargetLanguageCode,
            MasterTMGuid = masterGuid,
            PrimaryTMGuid = primaryGuid
        };

        await projectService.Service
            .SetProjectTMs2Async(Guid.Parse(project.ProjectGuid), new[] { tmAssignments });
    }
}