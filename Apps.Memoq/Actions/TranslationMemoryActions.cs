using Apps.Memoq.Models;
using Apps.Memoq.Models.Dto;
using Apps.Memoq.Models.ServerProjects.Requests;
using Apps.Memoq.Models.TranslationMemories.Requests;
using Apps.Memoq.Models.TranslationMemories.Responses;
using Apps.Memoq.Utils.FileUploader;
using Apps.MemoQ;
using Apps.MemoQ.Extensions;
using Apps.MemoQ.Models.TranslationMemories.Requests;
using Apps.MemoQ.Models.TranslationMemories.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using MQS.ServerProject;
using MQS.TM;
using System.Text;
using TMEngineType = MQS.TM.TMEngineType;
using TMOptimizationPreference = MQS.TM.TMOptimizationPreference;

namespace Apps.Memoq.Actions;

[ActionList("Translation memories")]
public class TranslationMemoryActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : MemoqInvocable(invocationContext)
{
    [Action("Search translation memories", Description = "Search translation memories")]
    public async Task<ListTranslationMemoriesResponse> ListTranslationMemories(
        [ActionParameter] LanguagesRequest input)
    {
        var filter = new TMListFilter
        {
            SourceLangCode = input.SourceLanguage,
            TargetLangCode = input.TargetLanguage,
            NameOrDescription = input.NameOrDescription,
            Client = input.Client,
            Domain = input.Domain,
            LastModifiedAfter = input.LastModifiedAfter,
            LastModifiedBefore = input.LastModifiedBefore,
            LastUsedAfter = input.LastUsedAfter,
            LastUsedBefore = input.LastUsedBefore,
            Subject = input.Subject,
            Project = input.Project ,
            UsedInProject = input.UsedInProject
        };

        var response = await ExecuteWithHandling(() => TmService.Service.ListTMs2Async(filter));
        var translationMemories = response.Select(x => new TmDto(x)).ToArray();

        return new ListTranslationMemoriesResponse
        {
            TranslationMemories = translationMemories
        };
    }

    [Action("Create translation memory", Description = "Create translation memory")]
    public async Task<TmDto> CreateTranslationMemory([ActionParameter] CreateTranslationMemoryRequest input)
    {
        var tmGuid = await ExecuteWithHandling(() => TmService.Service.CreateAndPublishAsync(new()
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
        }));

        var response = await ExecuteWithHandling(() => TmService.Service.GetTMInfoAsync(tmGuid));
        return new(response);
    }
    
    [Action("Get translation memory", Description = "Get details of a specific translation memory")]
    public async Task<TmDto> GetTranslationMemory([ActionParameter] TranslationMemoryRequest input)
    {
        var response = await ExecuteWithHandling(() => TmService.Service.GetTMInfoAsync(GuidExtensions.ParseWithErrorHandling(input.TmGuid)));
        return new(response);
    }   
    
    [Action("Delete translation memory", Description = "Delete specific translation memory")]
    public async Task DeleteTranslationMemory([ActionParameter] TranslationMemoryRequest input)
    {
        await ExecuteWithHandling(() => TmService.Service.DeleteTMAsync(GuidExtensions.ParseWithErrorHandling(input.TmGuid)));
    }    
    
    [Action("Update translation memory properties", Description = "Update specific translation memory properties")]
    public async Task UpdateTranslationMemoryProperties(
        [ActionParameter] TranslationMemoryRequest tm,
        [ActionParameter] UpdateTranslationMemoryPropertiesRequest input)
    {
        await ExecuteWithHandling(() => TmService.Service.UpdatePropertiesAsync(new()
        {
            Guid = GuidExtensions.ParseWithErrorHandling(tm.TmGuid),
            Name = input.Name,
            Client = input.Client,
            Domain = input.Domain,
            Subject = input.Subject,
            Project = input.Project,
            OptimizationPreference = EnumParser.Parse<TMOptimizationPreference>(input.OptimizationPreference,
                nameof(input.OptimizationPreference)) ?? default,
            StoreDocumentFullPath = input.StoreDocumentFullPath ?? default,
            StoreDocumentName = input.StoreDocumentName ?? default,
        }));
    }

    [Action("Import TMX file", Description = "Import TMX file")]
    public async Task<ImportTmxFileResponse> ImportTmxFile([ActionParameter] ImportTmxFileRequest input)
    {
        var file = await fileManagementClient.DownloadAsync(input.File);
        var fileBytes = await file.GetByteData();
        var result = FileUploader.UploadFile(fileBytes, TmxUploadManager, input.TmGuid);

        return new()
        {
            Guid = result.ToString()
        };
    }
    
    [Action("Import translation memory scheme from XML", Description = "Import specific translation memory's scheme from XML")]
    public async Task<ImportTmSchemeFromXmlResponse> ImportTmSchemeFromXml([ActionParameter] ImportTmSchemeRequest input)
    {
        var file = await fileManagementClient.DownloadAsync(input.File);
        var fileBytes = await file.GetByteData();
        var xml = Encoding.UTF8.GetString(fileBytes);
        var response = await ExecuteWithHandling(() => TmService.Service.ImportTMMetadataSchemeFromXMLAsync(GuidExtensions.ParseWithErrorHandling(input.TmGuid), xml));

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
        Guid masterGuid = translationMemoryRequest.MasterTmGuid != null
            ? GuidExtensions.ParseWithErrorHandling(translationMemoryRequest.MasterTmGuid)
            : Guid.Empty;
        
        Guid primaryGuid = translationMemoryRequest.PrimaryTmGuid != null
            ? GuidExtensions.ParseWithErrorHandling(translationMemoryRequest.PrimaryTmGuid)
            : Guid.Empty;
        
        var tmGuids = translationMemoryRequest.TmGuids ?? new List<string>();

        var tmAssignments = new ServerProjectTMAssignmentsForTargetLang
        {
            TMGuids = tmGuids.Select((x) => GuidExtensions.ParseWithErrorHandling(x)).ToArray(),
            TargetLangCode = translationMemoryRequest.TargetLanguageCode,
            MasterTMGuid = masterGuid,
            PrimaryTMGuid = primaryGuid
        };

        await ExecuteWithHandling(() => ProjectService.Service.SetProjectTMs2Async(GuidExtensions.ParseWithErrorHandling(project.ProjectGuid), new[] { tmAssignments }));
    }

    [Action("Export translation memory", Description = "Export translation memory as TMX file")]
    public async Task<ExportTranslationMemoryResponse> ExportTranslationMemory(
    [ActionParameter] ExportTranslationMemoryRequest input)
    {
        var tmGuid = GuidExtensions.ParseWithErrorHandling(input.TmGuid);
        var sessionId = Guid.Empty;

        try
        {
            sessionId = await ExecuteWithHandling(() => TmService.Service.BeginChunkedTMXExportAsync(tmGuid));

            await using var outputStream = new MemoryStream();

            while (true)
            {
                var chunk = await ExecuteWithHandling(() => TmService.Service.GetNextTMXChunkAsync(sessionId));

                if (chunk == null || chunk.Length == 0)
                    break;

                await outputStream.WriteAsync(chunk, 0, chunk.Length);
            }

            outputStream.Position = 0;

            var fileName = string.IsNullOrWhiteSpace(input.FileName)
                ? $"translation-memory-{input.TmGuid}.tmx"
                : EnsureTmxExtension(input.FileName);

            var fileReference = await fileManagementClient.UploadAsync(outputStream, "text/xml", fileName);

            return new ExportTranslationMemoryResponse
            {
                File = fileReference
            };
        }
        finally
        {
            if (sessionId != Guid.Empty)
            {
                await ExecuteWithHandling(() => TmService.Service.EndChunkedTMXExportAsync(sessionId));
            }
        }
    }

    private static string EnsureTmxExtension(string fileName)
    {
        return fileName.EndsWith(".tmx", StringComparison.OrdinalIgnoreCase)
            ? fileName
            : $"{fileName}.tmx";
    }
}