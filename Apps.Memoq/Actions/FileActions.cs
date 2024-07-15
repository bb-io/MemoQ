using System.Net.Mime;
using System.Xml.Linq;
using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.Memoq.Models.Dto;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using MQS.FileManager;
using MQS.ServerProject;
using Apps.Memoq.Models.ServerProjects.Requests;
using Apps.Memoq.Models.Files.Requests;
using Apps.Memoq.Models.Files.Responses;
using Apps.Memoq.Utils.FileUploader;
using Apps.Memoq.Utils.FileUploader.Managers;
using Apps.Memoq.Utils.Xliff;
using Blackbird.Applications.Sdk.Common.Files;
using MQS.TasksService;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Blackbird.Xliff.Utils;
using RestSharp;
using System.Collections;
using Blackbird.Xliff.Utils.Extensions;
using Blackbird.Xliff.Utils.Models;
using System.Text;
using System.Text.RegularExpressions;
using Apps.MemoQ.Models.Files.Requests;
using Apps.MemoQ.Models.Dto;

namespace Apps.Memoq.Actions;

[ActionList]
public class FileActions : BaseInvocable
{
    private readonly IFileManagementClient _fileManagementClient;

    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public FileActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
        : base(invocationContext)
    {
        _fileManagementClient = fileManagementClient;
    }

    [Action("List project translation documents", Description = "Returns the current translation documents of the specified project.")]
    public ListAllProjectFilesResponse ListAllProjectFiles(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] ListProjectFilesRequest input)
    {
        using var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);
        var response = projectService.Service
            .ListProjectTranslationDocuments2(Guid.Parse(project.ProjectGuid), new()
            {
                FillInAssignmentInformation = input.FillInAssignmentInformation ?? default,
            });

        var files = response.Select(x => new FileInfoDto(x)).ToArray();
        return new()
        {
            Files = files
        };
    }

    [Action("Get document", Description = "Get project file info by guid")]
    public FileDto GetFile(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] [Display("File GUID")]
        string fileGuid)
    {
        var files = ListAllProjectFiles(project, new());
        return files.Files.FirstOrDefault(f => f.Guid == fileGuid)
               ?? throw new($"No file found with the provided ID: {fileGuid}");
    }

    [Action("Delete document", Description = "Delete specific document from project")]
    public void DeleteFile([ActionParameter] DeleteFileRequest input)
    {
        using var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        projectService.Service
            .DeleteTranslationDocument(Guid.Parse(input.ProjectGuid), Guid.Parse(input.FileGuid));
    }

    [Action("Slice document", Description = "Slice specific document based on the specified options")]
    public async Task SliceDocument([ActionParameter] ProjectRequest project, [ActionParameter] SliceFileRequest input)
    {
        using var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        var options = new SliceDocumentRequest()
        {
            DocumentGuid = Guid.Parse(input.DocumentGuid),
            NumberOfParts = input.NumberOfParts,
            MeasurementUnit =
                EnumParser.Parse<SlicingMeasurementUnit>(input.SlicingMeasurementUnit,
                    nameof(input.SlicingMeasurementUnit))!.Value
        };

        await projectService.Service
            .SliceDocumentAsync(Guid.Parse(project.ProjectGuid), options);
    }

    [Action("Overwrite document", Description = "Overwrite specific document in project")]
    public async Task OverwriteFileInProject([ActionParameter] OverwriteFileInProjectRequest input)
    {
        using var fileService = new MemoqServiceFactory<IFileManagerService>(
            SoapConstants.FileServiceUrl, Creds);
        using var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        var manager = new FileUploadManager(fileService.Service);
        var file = await _fileManagementClient.DownloadAsync(input.File);
        var fileBytes = await file.GetByteData();
        var uploadFileResult = FileUploader.UploadFile(fileBytes, manager, input.Filename ?? input.File.Name);
        await projectService.Service.ReImportTranslationDocumentsAsync(Guid.Parse(input.ProjectGuid),
            new ReimportDocumentOptions[]
            {
                new()
                {
                    DocumentsToReplace = new[] { Guid.Parse(input.DocumentToReplaceGuid) },
                    FileGuid = uploadFileResult,
                    KeepUserAssignments = input.KeepAssignments ?? default,
                    PathToSetAsImportPath = input.PathToSetAsImportPath
                }
            },
            null
        );
    }

    [Action("Assign document to user", Description = "Assign document to a specific user")]
    public void AssignFileToUser([ActionParameter] AssignFileToUserRequest input)
    {
        using var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);
        var assignments = new ServerProjectTranslationDocumentUserAssignments[]
        {
            new()
            {
                DocumentGuid = Guid.Parse(input.FileGuid),
                UserRoleAssignments = new TranslationDocumentUserRoleAssignment[]
                {
                    new()
                    {
                        UserGuid = Guid.Parse(input.UserGuid),
                        DeadLine = input.Deadline,
                        DocumentAssignmentRole = input.Role is not null ? int.Parse(input.Role) : default
                    }
                }
            }
        };

        projectService.Service
            .SetProjectTranslationDocumentUserAssignments(Guid.Parse(input.ProjectGuid), assignments);
    }

    [Action("Import document", Description = "Uploads and imports a document to a project")]
    public async Task<UploadFileResponse> UploadAndImportFileToProject(
        [ActionParameter] UploadDocumentToProjectWithOptionsRequest request)
    {
        using var fileService = new MemoqServiceFactory<IFileManagerService>(
            SoapConstants.FileServiceUrl, Creds);
        using var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        string fileName = request.FileName ?? request.File.Name;

        var manager = new FileUploadManager(fileService.Service);

        var fileStream = await _fileManagementClient.DownloadAsync(request.File);
        var file = new MemoryStream();
        await fileStream.CopyToAsync(file);

        file.Position = 0;
        var fileBytes = await file.GetByteData();

        var uploadFileResult =
            FileUploader.UploadFile(fileBytes, manager, fileName);

        var options = new ImportTranslationDocumentOptions
        {
            FileGuid = uploadFileResult,
            TargetLangCodes = request.TargetLanguageCodes?.ToArray(),
            CreatePreview = request.PreviewCreation == null ? PreviewCreation.UseProjectPreference : (PreviewCreation) int.Parse(request.PreviewCreation),
            ExternalDocumentId = request.ExternalDocumentId
        };

        if (request.ImportEmbeddedImages != null) options.ImportEmbeddedImages = (bool)request.ImportEmbeddedImages;
        if (request.ImportEmbeddedObjects != null) options.ImportEmbeddedObjects = (bool)request.ImportEmbeddedObjects;
        if (request.FilterConfigResGuid != null)
        {
            options.FilterConfigResGuid = Guid.Parse(request.FilterConfigResGuid);
            string? importSettings = null;
            if (fileName.EndsWith(".xliff"))
            {
                file.Position = 0;
                var reader = new StreamReader(file);
                importSettings = reader.ReadToEnd();
            }
            options.ImportSettingsXML = importSettings;
        }


        var results = await projectService.Service
            .ImportTranslationDocumentsWithOptionsAsync(
                Guid.Parse(request.ProjectGuid),
                new List<ImportTranslationDocumentOptions> { options }.ToArray());

        foreach(var result in results)
        {
            if (result.ResultStatus == ResultStatus.Error)
            {
                throw new InvalidOperationException(
                    $"Error while importing file, result status: {result.ResultStatus}, message: {result.DetailedMessage}");
            }
        }
        

        return new()
        {
            // Right now we have 1 target language, so 1 document GUID. If we have multiple target files, this should be changed as well or we need an extra action
            DocumentGuid = results.FirstOrDefault()?.DocumentGuids.Select(x => x.ToString()).FirstOrDefault()
        };
    }

    [Action("Re-import document", Description = "Uploads and re-imports a document to a project")]
    public async Task<UploadFileResponse> UploadAndReimportFileToProject(
        [ActionParameter] UploadDocumentToProjectRequest request,
        [ActionParameter] ReimportDocumentsRequest reimportDocumentsRequest)
    {
        var fileService = new MemoqServiceFactory<IFileManagerService>(SoapConstants.FileServiceUrl, Creds);
        var projectService = new MemoqServiceFactory<IServerProjectService>(SoapConstants.ProjectServiceUrl, Creds);

        var fileStream = await _fileManagementClient.DownloadAsync(request.File);
        var file = new MemoryStream();
        await fileStream.CopyToAsync(file);
        file.Position = 0;
        
        var fileBytes = await file.GetByteData();

        var uploadFileResult = FileUploader.UploadFile(fileBytes, new FileUploadManager(fileService.Service), request.FileName ?? request.File.Name);

        string? importSettings = null;
        if ((request.FileName ?? request.File.Name).EndsWith(".xliff"))
        {
            file.Position = 0;
            importSettings = await new StreamReader(file).ReadToEndAsync();
        }

        var result = await projectService.Service.ReImportTranslationDocumentsAsync(Guid.Parse(request.ProjectGuid),
            new[]
            {
                new ReimportDocumentOptions
                {
                    DocumentsToReplace = [Guid.Parse(reimportDocumentsRequest.DocumentGuid)],
                    FileGuid = uploadFileResult,
                    KeepUserAssignments = reimportDocumentsRequest.KeepUserAssignments ?? default,
                    PathToSetAsImportPath = reimportDocumentsRequest.PathToSetAsImportPath ?? string.Empty
                },
            }, importSettings);

        var first = result.FirstOrDefault(x => x.ResultStatus == ResultStatus.Success);
        if (first is null)
        {
            throw new InvalidOperationException("No successful reimport found");
        }

        return new UploadFileResponse
        {
            DocumentGuid = first.DocumentGuids.Select(x => x.ToString()).First()
        };
    }
    [Action ("Update document from XLIFF", Description = "Update translation document from exported XLIFF")]
    public async Task<UploadFileResponse> UpdateDocumentFromXliff(
        [ActionParameter] UploadDocumentToProjectRequest request,
        [ActionParameter] UpdateFromXliffRequest XliffRequest)
    {        
        var mqXliffFileResponse = await DownloadFileAsXliff(
            new GetDocumentRequest
            {
                ProjectGuid = request.ProjectGuid,
                DocumentGuid = XliffRequest.DocumentGuid
            },
            new DownloadXliffRequest
            {
                FullVersionHistory = false,
                UseMqxliff = true
            });

        var file = await _fileManagementClient.DownloadAsync(request.File);
        byte[] fileBytes =  await ProcessXliffFile(file, request.File.Name);

        var mqXliffFile = await _fileManagementClient.DownloadAsync(mqXliffFileResponse.File);
        
        var updatedMqXliffFile = UpdateMqxliffFile(mqXliffFile, new MemoryStream(fileBytes), XliffRequest.UpdateLocked.GetValueOrDefault(true), XliffRequest.UpdateConfirmed.GetValueOrDefault(true));
        string mqXliffFileName = (request.FileName ?? request.File.Name) + ".mqxliff";

        var fileService = new MemoqServiceFactory<IFileManagerService>(SoapConstants.FileServiceUrl, Creds);
        var projectService = new MemoqServiceFactory<IServerProjectService>(SoapConstants.ProjectServiceUrl, Creds);

        updatedMqXliffFile.Position = 0;
        var bytes = await updatedMqXliffFile.GetByteData();

        var uploadFileResult = FileUploader.UploadFile(bytes, new FileUploadManager(fileService.Service), mqXliffFileName);

        var result = await projectService.Service.UpdateTranslationDocumentFromBilingualAsync(Guid.Parse(request.ProjectGuid),
            uploadFileResult, BilingualDocFormat.XLIFF);

        var first = result.FirstOrDefault(x => x.ResultStatus == ResultStatus.Success);
        if (first is null)
        {
            throw new InvalidOperationException("No successful reimport found");
        }

        return new UploadFileResponse
        {
            DocumentGuid = first.DocumentGuids.Select(x => x.ToString()).First()
        };
    }
    [Action("Import document as XLIFF", Description = "Uploads and imports a document to a project as XLIFF")]
    public async Task<UploadFileResponse> UploadAndImportFileToProjectAsXliff(
        [ActionParameter] UploadDocumentToProjectRequest request,
        [ActionParameter] ImportDocumentAsXliffRequest importDocumentAsXliffRequest)
    {
        var file = await _fileManagementClient.DownloadAsync(request.File);
        byte[] fileBytes = request.File.Name.EndsWith(".xliff") ? await ProcessXliffFile(file, request.File.Name) : await file.GetByteData();

        var fileName = request.FileName ?? request.File.Name;
        string fileReferenceName = string.Empty;
        if(fileName.EndsWith(".xliff"))
        {
            fileReferenceName = fileName.Replace(".xliff", "-2.1.xliff");
        }
        
        fileName = string.IsNullOrEmpty(fileReferenceName) ? fileName : fileReferenceName;
        var xliffFileReference = await _fileManagementClient.UploadAsync(new MemoryStream(fileBytes), MediaTypeNames.Application.Xml, fileName);

        return string.IsNullOrEmpty(importDocumentAsXliffRequest.DocumentGuid) 
            ? await UploadAndImportFileToProject(new UploadDocumentToProjectWithOptionsRequest { File = xliffFileReference, ProjectGuid = request.ProjectGuid, TargetLanguageCodes = request.TargetLanguageCodes, FileName = request.FileName }) 
            : await ReimportDocumentAsync(importDocumentAsXliffRequest, xliffFileReference, request);
    }

    [Action("Export document", Description = "Exports and downloads a document with options")]
    public async Task<DownloadFileResponse> DownloadFileByGuid(
        [ActionParameter] DownloadFileRequest request)
    {
        using var fileService = new MemoqServiceFactory<IFileManagerService>(
            SoapConstants.FileServiceUrl, Creds);
        using var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        var exportResult = await projectService.Service
            .ExportTranslationDocument2Async(Guid.Parse(request.ProjectGuid), Guid.Parse(request.DocumentGuid),
                new DocumentExportOptions
                {
                    ExportAllMultilingualSiblings = request.ExportAllMultilingualSibling ?? true,
                    RevertFaultyTargetsToSource = request.RevertFaultyTargetsToSource ?? false,
                    CopySourceToEmptyTarget = request.CopySourceToEmptyTarget ?? false,
                    CopySourceToUnconfirmedRows = request.CopySourceToEmptyTarget ?? false,
                });

        var data = DownloadFile(fileService.Service, exportResult.FileGuid, out var filename);
        var document = GetFile(new()
        {
            ProjectGuid = request.ProjectGuid
        }, request.DocumentGuid);

        using var stream = new MemoryStream(data);
        var fileReference = await _fileManagementClient.UploadAsync(stream, MediaTypeNames.Application.Octet, filename);
        return new(document) { File = fileReference };
    }

    [Action("Export document as XLIFF",
        Description = "Exports and downloads the translation document as XLIFF (MQXLIFF) bilingual")]
    public async Task<DownloadFileResponse> DownloadFileAsXliff(
        [ActionParameter] GetDocumentRequest documentRequest,
        [ActionParameter] DownloadXliffRequest request)
    {
        using var fileService = new MemoqServiceFactory<IFileManagerService>(
            SoapConstants.FileServiceUrl, Creds);
        using var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        var fullVersion = request.FullVersionHistory ?? false;
        var includeSkeleton = request.IncludeSkeleton ?? false;

        var exportResult = await projectService.Service
            .ExportTranslationDocumentAsXliffBilingualAsync(Guid.Parse(documentRequest.ProjectGuid),
                Guid.Parse(documentRequest.DocumentGuid), new XliffBilingualExportOptions
                {
                    FullVersionHistory = fullVersion,
                    IncludeSkeleton = includeSkeleton,
                    SaveCompressed = fullVersion || includeSkeleton || (request.SaveCompressed ?? false)
                });

        var data = DownloadFile(fileService.Service, exportResult.FileGuid, out var filename);
        var document = GetFile(new()
        {
            ProjectGuid = documentRequest.ProjectGuid
        }, documentRequest.DocumentGuid);

        using var stream = new MemoryStream(data);

        var useMqxliff = request.UseMqxliff ?? false;
        if (useMqxliff)
        {
            var fileReference = await _fileManagementClient.UploadAsync(stream, "application/xliff+xml", filename);
            return new(document) { File = fileReference };
        }

        var fileReferenceXliff = await ConvertMqXliffToXliff(stream, filename.Replace(".mqxliff", ".xliff"));
        return new(document) { File = fileReferenceXliff };
    }

    [Action("Get document analysis", Description = "Get analysis for a specific document")]
    public GetAnalysisResponse GetAnalysisForFile(
        [ActionParameter] GetAnalysisForFileRequest input)
    {
        using var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        var task = projectService.Service.StartStatisticsOnTranslationDocumentsTask2(
            new()
            {
                ProjectGuid = Guid.Parse(input.ProjectGuid),
                DocumentOrSliceGuids = new[] { Guid.Parse(input.DocumentGuid) },
                ResultFormat =
                    EnumParser.Parse<StatisticsResultFormat>(input.Format, nameof(input.Format)) ?? default,
                Options = new()
                {
                    Algorithm = EnumParser.Parse<StatisticsAlgorithm>(input.Algorithm, nameof(input.Algorithm)) ??
                                default,
                    Analysis_Homogenity = input.AnalysisHomogenity ?? default,
                    Analysis_ProjectTMs = input.AnalysisProjectTMs ?? default,
                    Analyzis_DetailsByTM = input.AnalysisDetailsByTm ?? default,
                    DisableCrossFileRepetition = input.DisableCrossFileRepetition ?? default,
                    IncludeLockedRows = input.IncludeLockedRows ?? default,
                    RepetitionPreferenceOver100 = input.RepetitionPreferenceOver100 ?? default,
                    ShowCounts = input.ShowCounts ?? default,
                    ShowCounts_IncludeTargetCount = input.ShowCountsIncludeTargetCount ?? default,
                    ShowCounts_IncludeWhitespacesInCharCount =
                        input.ShowCountsIncludeWhitespacesInCharCount ?? default,
                    ShowCounts_StatusReport = input.ShowCountsStatusReport ?? default,
                    ShowResultsPerFile = input.ShowResultsPerFile ?? default,
                    TagCharWeight = input.TagCharWeight ?? default,
                    TagWordWeight = input.TagWordWeight ?? default,
                }
            });

        var taskResult = (StatisticsTaskResult)WaitForAsyncTaskToFinishAndGetResult(task.TaskId);
        var document = GetFile(input, input.DocumentGuid);
        var statistics = taskResult.ResultsForTargetLangs.First();

        return new(
            statistics,
            _fileManagementClient,
            $"{Path.GetFileNameWithoutExtension(document.Name)}_{statistics.TargetLangCode}.html",
            MediaTypeNames.Text.Html);
    }

    [Action("Get project analysis", Description = "Get analysis for all project documents")]
    public GetAnalysesForAllDocumentsResponse GetAnalysesForAllDocuments(
        [ActionParameter] GetAnalysisForProjectRequest input)
    {
        using var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);
        var task = projectService.Service.StartStatisticsOnProjectTask2(new()
        {
            ProjectGuid = Guid.Parse(input.ProjectGuid),
            ResultFormat =
                EnumParser.Parse<StatisticsResultFormat>(input.Format, nameof(input.Format)) ?? default,
            Options = new()
            {
                Algorithm = EnumParser.Parse<StatisticsAlgorithm>(input.Algorithm, nameof(input.Algorithm)) ?? default,
                Analysis_Homogenity = input.AnalysisHomogenity ?? default,
                Analysis_ProjectTMs = input.AnalysisProjectTMs ?? default,
                Analyzis_DetailsByTM = input.AnalysisDetailsByTm ?? default,
                DisableCrossFileRepetition = input.DisableCrossFileRepetition ?? default,
                IncludeLockedRows = input.IncludeLockedRows ?? default,
                RepetitionPreferenceOver100 = input.RepetitionPreferenceOver100 ?? default,
                ShowCounts = input.ShowCounts ?? default,
                ShowCounts_IncludeTargetCount = input.ShowCountsIncludeTargetCount ?? default,
                ShowCounts_IncludeWhitespacesInCharCount = input.ShowCountsIncludeWhitespacesInCharCount ?? default,
                ShowCounts_StatusReport = input.ShowCountsStatusReport ?? default,
                ShowResultsPerFile = input.ShowResultsPerFile ?? default,
                TagCharWeight = input.TagCharWeight ?? default,
                TagWordWeight = input.TagWordWeight ?? default,
            }
        });

        var taskResult = (StatisticsTaskResult)WaitForAsyncTaskToFinishAndGetResult(task.TaskId);
        var analysis = taskResult.ResultsForTargetLangs
            .Select(x => new GetAnalysisResponse(x, _fileManagementClient, $"Analysis_{x.TargetLangCode}.html",
                MediaTypeNames.Text.Html))
            .ToList();

        return new()
        {
            Analyses = analysis
        };
    }

    [Action("Apply translated content to updated source",
        Description = "Apply translated content to updated source")]
    public ApplyTranslatedContentToUpdatedSourceResponse ApplyTranslatedContentToUpdatedSource(
        [ActionParameter] ApplyTranslatedContentToUpdatedSourceRequest input)
    {
        using var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        var taskInfo = projectService.Service.StartXTranslateTask(Guid.Parse(input.ProjectGuid),
            new()
            {
                XTranslateScenario = EnumParser.Parse<XTranslateScenario>(input.XTranslateScenario,
                    nameof(input.XTranslateScenario)) ?? default,

                WorkWithContextIds = input.WorkWithContextIds ?? default,
                DocInfos = new XTranslateDocInfo[]
                {
                    new()
                    {
                        DocumentGuid = Guid.Parse(input.DocumentGuid)
                    }
                },
                NewRevisionOptions = new()
                {
                    ExpectedFinalState = EnumParser.Parse<ExpectedFinalStateAfterXTranslate>(
                        input.ExpectedFinalState, nameof(input.ExpectedFinalState)) ?? default,
                    SourceFilter = EnumParser.Parse<ExpectedSourceStateBeforeXTranslate>(input.SourceFilter,
                        nameof(input.SourceFilter)) ?? default,

                    InsertEmptyTranslations = input.InsertEmptyTranslations ?? default,
                    LockXTranslatedRows = input.LockXTranslatedRows ?? default,
                }
            });

        var taskResult = (XTranslateTaskResult)WaitForAsyncTaskToFinishAndGetResult(taskInfo.TaskId);
        var documents = taskResult.DocumentResults
            .Select(x => new XTranslateDocumentDto(x))
            .ToArray();

        return new()
        {
            Results = documents
        };
    }

    [Action("Deliver document", Description = "Deliver a specific document")]
    public async Task DeliverDocument([ActionParameter] ProjectRequest project,
        [ActionParameter] DeliverDocumentInput input)
    {
        var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        var request = new DeliverDocumentRequest
        {
            DocumentGuid = Guid.Parse(input.DocumentGuid),
            DeliveringUserGuid = Guid.Parse(input.DeliveringUserGuid),
            ReturnDocToPreviousActor = input.ReturnDocToPreviousActorField ?? default,
        };

        await projectService.Service.DeliverDocumentAsync(Guid.Parse(project.ProjectGuid), request);
    }

    private byte[] DownloadFile(IFileManagerService fmService, Guid fileGuid, out string fileName)
    {
        const int chunkSize = 500000;
        byte[] chunkBytes;

        var downloadResponse = fmService.BeginChunkedFileDownload(new(fileGuid, false));
        fileName = downloadResponse.fileName;

        var fileBytesLeft = downloadResponse.fileSize;
        var result = new byte[fileBytesLeft];

        while (fileBytesLeft > 0)
        {
            chunkBytes = fmService.GetNextFileChunk(downloadResponse.BeginChunkedFileDownloadResult, chunkSize);
            Array.Copy(chunkBytes, 0, result, downloadResponse.fileSize - fileBytesLeft, chunkBytes.Length);
            fileBytesLeft -= chunkBytes.Length;
        }

        if (downloadResponse.BeginChunkedFileDownloadResult != Guid.Empty)
            fmService.EndChunkedFileDownload(downloadResponse.BeginChunkedFileDownloadResult);

        fmService.DeleteFile(fileGuid);

        return result;
    }

    private TaskResult WaitForAsyncTaskToFinishAndGetResult(Guid taskId)
    {
        using var taskService = new MemoqServiceFactory<ITasksService>(
            SoapConstants.TaskServiceUrl, Creds);
        var taskStatus = taskService.Service.GetTaskStatus(taskId).Status;
        var i = 1;
        while (taskStatus == MQS.TasksService.TaskStatus.Pending
               || taskStatus == MQS.TasksService.TaskStatus.Executing)
        {
            if (i < 16)
                i = 2 * i;
            Thread.Sleep(i * 1000);
            taskStatus = taskService.Service.GetTaskStatus(taskId).Status;
        }

        if (taskStatus != MQS.TasksService.TaskStatus.Completed)
            throw new Exception($"Task has status {taskStatus.ToString()}.");

        return taskService.Service.GetTaskResult(taskId);
    }

    private async Task<byte[]> ConvertTo2_1Xliff(XDocument xDocument, string fileName)
    {
        var xliffFile = xDocument.ConvertToTwoPointOne();

        var xliffStream = new MemoryStream();
        xliffFile.Save(xliffStream);

        xliffStream.Position = 0;
        return await xliffStream.GetByteData();
    }

    private async Task<FileReference> ConvertMqXliffToXliff(Stream stream, string fileName, bool useSkeleton = false)
    {
        var xliffFile = stream.ConvertMqXliffToXliff(useSkeleton);
        byte[] byteArray = Encoding.UTF8.GetBytes(xliffFile);
        var xliffStream = new MemoryStream(byteArray);

        xliffStream.Position = 0;
        
        string contentType = MediaTypeNames.Text.Plain;
        
        return await _fileManagementClient.UploadAsync(xliffStream, contentType, fileName);
    }
    
    private async Task<byte[]> ProcessXliffFile(Stream file, string fileName)
    {
        var xDocument = XDocument.Load(file);
        string version = xDocument.GetXliffVersion();

        if (version == "1.2")
        {
            return await ConvertTo2_1Xliff(xDocument, fileName);
        }
        else if (version == "2.1")
        {
            return await file.GetByteData();
        }

        throw new("Unsupported XLIFF version. Currently only 1.2 and 2.1 are supported.");
    }

    private async Task<UploadFileResponse> ReimportDocumentAsync(
        ImportDocumentAsXliffRequest importDocumentAsXliffRequest,
        FileReference fileReference,
        UploadDocumentToProjectRequest request)
    {
        if (importDocumentAsXliffRequest.UpdateSegmentStatuses != null && importDocumentAsXliffRequest.UpdateSegmentStatuses.Value)
        {
            var mqXliffFileResponse = await DownloadFileAsXliff(
                new GetDocumentRequest
                {
                    ProjectGuid = request.ProjectGuid,
                    DocumentGuid = importDocumentAsXliffRequest.DocumentGuid ??
                                   throw new("Can not reimport without document guid")
                },
                new DownloadXliffRequest
                {
                    FullVersionHistory = false,
                    UseMqxliff = true
                });

            var mqXliffFile = await _fileManagementClient.DownloadAsync(mqXliffFileResponse.File);
            var fileStream = await _fileManagementClient.DownloadAsync(fileReference);
            
            var updatedMqXliffFile = UpdateMqxliffFile(mqXliffFile, fileStream);
            string mqXliffFileName = request.FileName ?? request.File.Name + ".mqxliff";
            fileReference = await _fileManagementClient.UploadAsync(updatedMqXliffFile, MediaTypeNames.Application.Xml,
                mqXliffFileName);
        }

        return await UploadAndReimportFileToProject(new UploadDocumentToProjectRequest
        {
            File = fileReference,
            ProjectGuid = request.ProjectGuid,
            TargetLanguageCodes = request.TargetLanguageCodes,
            FileName = request.FileName
        }, new ReimportDocumentsRequest
        {
            DocumentGuid = importDocumentAsXliffRequest.DocumentGuid,
            KeepUserAssignments = importDocumentAsXliffRequest.KeepUserAssignments,
            PathToSetAsImportPath = importDocumentAsXliffRequest.PathToSetAsImportPath
        });
    }

    private Stream UpdateMqxliffFile(Stream mqXliffFile, Stream xliffFile, bool locked = true, bool confirmed = true)
    {
        XNamespace nsXliff = "urn:oasis:names:tc:xliff:document:1.2";
        XNamespace nsXliff21 = "urn:oasis:names:tc:xliff:document:2.0";

        var xliffDoc = XDocument.Load(xliffFile);
        var mqXliffDoc = XDocument.Load(mqXliffFile);

        var xliffUnits = xliffDoc.Descendants(nsXliff21 + "unit");

        foreach (var mqTransUnit in mqXliffDoc.Descendants(nsXliff + "trans-unit"))
        {
            var id = mqTransUnit.Attribute("id")?.Value;
            var mqTarget = mqTransUnit.Elements(nsXliff + "target").FirstOrDefault();

            var xliffUnit = xliffUnits.FirstOrDefault(x => x.Attribute("id")?.Value == id);
            var xliffTargetNodes = xliffUnit?.Elements(nsXliff21 + "segment").Elements(nsXliff21 + "target").Nodes();

            if (mqTarget != null && xliffTargetNodes != null && !mqTarget.Nodes().SequenceEqual(xliffTargetNodes, new XNodeEqualityComparer()))
            {
                if (!locked && mqTransUnit.Attribute(XNamespace.Get("MQXliff") + "locked")?.Value == "locked") { continue; }
                if (!confirmed && mqTransUnit.Attribute(XNamespace.Get("MQXliff") + "status")?.Value == "Proofread") { continue; }
                mqTarget.RemoveAll();
                mqTarget.Add(xliffTargetNodes);
                mqTransUnit.SetAttributeValue(XNamespace.Get("MQXliff") + "status", "Edited");
            }
        }
        
        var updatedString = Regex.Replace(mqXliffDoc.ToString(), "(<(source(.*?)|\\/bpt|\\/ph|target(.*?))>)\\r?\\n\\s+(?!\\s?(<target|<\\/trans-unit>))", "${1}").Replace("& ","&amp; ");
        var updatedMqXliffStream = new MemoryStream(Encoding.UTF8.GetBytes(updatedString));
        updatedMqXliffStream.Position = 0;
        return updatedMqXliffStream;
    }
}