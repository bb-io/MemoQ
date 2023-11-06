﻿using System.Net.Mime;
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
using MQS.TasksService;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Parsers;

namespace Apps.Memoq.Actions;

[ActionList]
public class FileActions : BaseInvocable
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public FileActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("List project documents", Description = "List all project documents")]
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

        var files = response.Select(x => new FileDto(x)).ToArray();
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
    public void OverwriteFileInProject([ActionParameter] OverwriteFileInProjectRequest input)
    {
        using var fileService = new MemoqServiceFactory<IFileManagerService>(
            SoapConstants.FileServiceUrl, Creds);
        using var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        var manager = new FileUploadManager(fileService.Service);
        var uploadFileResult = FileUploader.UploadFile(input.File.Bytes, manager, input.Filename ?? input.File.Name);
        projectService.Service.ReImportTranslationDocuments(Guid.Parse(input.ProjectGuid),
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
    public UploadFileResponse UploadAndImportFileToProject(
        [ActionParameter] UploadDocumentToProjectRequest request)
    {
        using var fileService = new MemoqServiceFactory<IFileManagerService>(
            SoapConstants.FileServiceUrl, Creds);
        using var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        var manager = new FileUploadManager(fileService.Service);
        var uploadFileResult =
            FileUploader.UploadFile(request.File.Bytes, manager, request.FileName ?? request.File.Name);
        var result = projectService.Service
            .ImportTranslationDocument(
                Guid.Parse(request.ProjectGuid),
                uploadFileResult,
                request.TargetLanguageCodes?.ToArray(),
                null);

        return new()
        {
            // Right now we have 1 target language, so 1 document GUID. If we have multiple target files, this should be changed as well or we need an extra action
            DocumentGuid = result.DocumentGuids.Select(x => x.ToString()).First()
        };
    }

    [Action("Export document", Description = "Exports and downloads a document with options")]
    public DownloadFileResponse DownloadFileByGuid(
        [ActionParameter] DownloadFileRequest request)
    {
        using var fileService = new MemoqServiceFactory<IFileManagerService>(
            SoapConstants.FileServiceUrl, Creds);
        using var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        var exportResult = projectService.Service
            .ExportTranslationDocument2(Guid.Parse(request.ProjectGuid), Guid.Parse(request.DocumentGuid),
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

        return new(document)
        {
            File = new(data)
            {
                Name = filename,
                ContentType = MediaTypeNames.Application.Octet
            }
        };
    }

    [Action("Export document as XLIFF",
        Description = "Exports and downloads the translation document as XLIFF (MQXLIFF) bilingual")]
    public DownloadFileResponse DownloadFileAsXliff(
        [ActionParameter] DownloadXliffRequest request)
    {
        using var fileService = new MemoqServiceFactory<IFileManagerService>(
            SoapConstants.FileServiceUrl, Creds);
        using var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        var fullVersion = request.FullVersionHistory ?? false;
        var includeSkeleton = request.IncludeSkeleton ?? false;

        var exportResult = projectService.Service
            .ExportTranslationDocumentAsXliffBilingual(Guid.Parse(request.ProjectGuid),
                Guid.Parse(request.DocumentGuid), new XliffBilingualExportOptions
                {
                    FullVersionHistory = fullVersion,
                    IncludeSkeleton = includeSkeleton,
                    SaveCompressed = fullVersion || includeSkeleton || (request.SaveCompressed ?? false)
                });

        var data = DownloadFile(fileService.Service, exportResult.FileGuid, out var filename);
        var document = GetFile(new()
        {
            ProjectGuid = request.ProjectGuid
        }, request.DocumentGuid);

        return new(document)
        {
            File = new(data)
            {
                Name = filename,
                ContentType = "application/xliff+xml",
            }
        };
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
            .Select(x => new GetAnalysisResponse(x, $"Analysis_{x.TargetLangCode}.html", MediaTypeNames.Text.Html))
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
}