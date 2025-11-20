using Apps.Memoq.Models.Dto;
using Apps.Memoq.Models.Files.Requests;
using Apps.Memoq.Models.Files.Responses;
using Apps.Memoq.Models.ServerProjects.Requests;
using Apps.Memoq.Utils.FileUploader;
using Apps.Memoq.Utils.Xliff;
using Apps.MemoQ;
using Apps.MemoQ.Extensions;
using Apps.MemoQ.Models.Dto;
using Apps.MemoQ.Models.Files.Requests;
using Apps.MemoQ.Models.Files.Responses;
using Apps.MemoQ.Models.ServerProjects.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using MQS.FileManager;
using MQS.ServerProject;
using MQS.TasksService;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Apps.Memoq.Actions;

[ActionList("Files")]
public class FileActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : MemoqInvocable(invocationContext)
{
    [Action("Search project files", Description = "Returns the files currently in a project.")]
    public async Task<ListAllProjectFilesResponse> ListAllProjectFiles(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] ListProjectFilesRequest input)
    {
        var response = await ExecuteWithHandling(() => ProjectService.Service
            .ListProjectTranslationDocuments2Async(GuidExtensions.ParseWithErrorHandling(project.ProjectGuid), new()
            {
                FillInAssignmentInformation = input.FillInAssignmentInformation ?? default,
            }));

        var files = response.Select(x => new FileInfoDto(x)).ToArray();
        return new()
        {
            Files = files
        };
    }

    [Action("Get file info", Description = "Get project file info")]
    public async Task<FileInfoDto> GetFile(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] [Display("File ID")]
        string fileGuid)
    {
        var files = await ListAllProjectFiles(project, new());
        return files.Files.FirstOrDefault(f => f.Guid == fileGuid)
               ?? throw new PluginMisconfigurationException ($"No file found with the provided ID: {fileGuid}");
    }

    [Action("Export an edit distance report as a CSV", Description = "Exports an edit distance report as a CSV and gets the results.")]
    public async Task<EditDistanceReportExportResponse> GetEditDistanceReport([ActionParameter] ProjectRequest project, [ActionParameter][Display("Report ID")] string reportId)
    {
        var projectGuid = GuidExtensions.ParseWithErrorHandling(project.ProjectGuid);
        var reportGuid = GuidExtensions.ParseWithErrorHandling(reportId);

        var csvResult = await ExecuteWithHandling(() =>
            ProjectService.Service.GetEditDistanceReportAsCsvAsync(projectGuid, reportGuid)
        );
        var filename = $"EditDistanceReport_{reportId}.csv";

        using var stream = new MemoryStream(csvResult.Content);
        var fileReference = await fileManagementClient.UploadAsync(stream, MimeTypes.GetMimeType(filename), filename);

        return new EditDistanceReportExportResponse(reportGuid, fileReference);
    }

    [Action("File exists", Description = "Check if the file exists in a specified project")]
    public async Task<DocumentExistsResponse> DocumentExists(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] DocumentExistsRequest input)
    {
        if (!input.IsValid())
            throw new PluginMisconfigurationException("You should specify at least one of the optional inputs to search files on");

        var response = await ExecuteWithHandling(() => ProjectService.Service
            .ListProjectTranslationDocuments2Async(GuidExtensions.ParseWithErrorHandling(project.ProjectGuid), new()));

        return new()
        {
            Result = response.Any(x =>
                (input.Guid is null || x.DocumentGuid.ToString() == input.Guid) && (input.ExternalDocumentId is null ||
                    x.ExternalDocumentId == input.ExternalDocumentId) && (input.TargetLanguageCode is null ||
                                                                          x.TargetLangCode ==
                                                                          input.TargetLanguageCode) &&
                (input.DocumentName is null || x.DocumentName == input.DocumentName) &&
                (input.WebTransUrl is null || x.WebTransUrl == input.WebTransUrl))
        };
    }

    [Action("Delete file", Description = "Delete a file from a project")]
    public async Task DeleteFile([ActionParameter] DeleteFileRequest input)
    {
        await ExecuteWithHandling(() => ProjectService.Service
            .DeleteTranslationDocumentAsync(GuidExtensions.ParseWithErrorHandling(input.ProjectGuid), GuidExtensions.ParseWithErrorHandling(input.FileGuid)));
    }

    [Action("Slice file", Description = "Slice a file based on the specified options")]
    public async Task SliceDocument([ActionParameter] ProjectRequest project, [ActionParameter] SliceFileRequest input)
    {
        var options = new SliceDocumentRequest()
        {
            DocumentGuid = GuidExtensions.ParseWithErrorHandling(input.DocumentGuid),
            NumberOfParts = input.NumberOfParts,
            MeasurementUnit =
                EnumParser.Parse<SlicingMeasurementUnit>(input.SlicingMeasurementUnit,
                    nameof(input.SlicingMeasurementUnit))!.Value
        };

        await ExecuteWithHandling(() => ProjectService.Service.SliceDocumentAsync(GuidExtensions.ParseWithErrorHandling(project.ProjectGuid), options));
    }

    [Action("Overwrite file", Description = "Overwrite a file in a project")]
    public async Task OverwriteFileInProject([ActionParameter] OverwriteFileInProjectRequest input)
    {
        var file = await fileManagementClient.DownloadAsync(input.File);
        var fileBytes = await file.GetByteData();
        var uploadFileResult = FileUploader.UploadFile(fileBytes, FileUploadManager, input.Filename ?? input.File.Name);
        await ExecuteWithHandling(() => ProjectService.Service.ReImportTranslationDocumentsAsync(GuidExtensions.ParseWithErrorHandling(input.ProjectGuid),
            new ReimportDocumentOptions[]
            {
                new()
                {
                    DocumentsToReplace = new[] { GuidExtensions.ParseWithErrorHandling(input.DocumentToReplaceGuid) },
                    FileGuid = uploadFileResult,
                    KeepUserAssignments = input.KeepAssignments ?? default,
                    PathToSetAsImportPath = input.PathToSetAsImportPath
                }
            },
            null
        ));
    }

    [Action("Assign file to user", Description = "Assign a file to a specific user")]
    public async Task AssignFileToUser([ActionParameter] AssignFileToUserRequest input)
    {
        var assignments = new ServerProjectTranslationDocumentUserAssignments[]
        {
            new()
            {
                DocumentGuid = GuidExtensions.ParseWithErrorHandling(input.FileGuid),
                UserRoleAssignments = new TranslationDocumentUserRoleAssignment[]
                {
                    new()
                    {
                        UserGuid = GuidExtensions.ParseWithErrorHandling(input.UserGuid),
                        DeadLine = input.Deadline,
                        DocumentAssignmentRole = input.Role is not null ? int.Parse(input.Role) : default
                    }
                }
            }
        };

        await ExecuteWithHandling(() => ProjectService.Service.SetProjectTranslationDocumentUserAssignmentsAsync(GuidExtensions.ParseWithErrorHandling(input.ProjectGuid), assignments));
    }

    [Action("Upload file", Description = "Uploads and imports a file to a project")]
    public async Task<UploadFileResponse> UploadAndImportFileToProject(
        [ActionParameter] UploadDocumentToProjectWithOptionsRequest request)
    {
        var targetLanguageCodes = request.TargetLanguageCodes;

        if (request.PreventDuplicateDocuments == true)
        {
            if (targetLanguageCodes == null)
            {
                var serverProjectActions = new ServerProjectActions(InvocationContext);
                var projectInfo = await serverProjectActions.GetProject(new ProjectRequest { ProjectGuid = request.ProjectGuid });
                targetLanguageCodes = projectInfo.TargetLanguageCodes;
            }

            var existingDocumentGuidsByLanguageCode = await GetExistingDocumentGuidsByLanguageCode(
                request.ProjectGuid,
                targetLanguageCodes,
                request.File.Name,
                request.ExternalDocumentId);

            targetLanguageCodes = targetLanguageCodes.Where(langCode => !existingDocumentGuidsByLanguageCode.ContainsKey(langCode));

            if (!targetLanguageCodes.Any())
            {
                return new UploadFileResponse { DocumentGuids = [] };
            }
        }

        var fileStream = await fileManagementClient.DownloadAsync(request.File);
        var file = new MemoryStream();
        await fileStream.CopyToAsync(file);

        file.Position = 0;
        var fileBytes = await file.GetByteData();

        var uploadFileResult = FileUploader.UploadFile(fileBytes, FileUploadManager, request.File.Name);

        var options = new ImportTranslationDocumentOptions
        {
            FileGuid = uploadFileResult,
            TargetLangCodes = targetLanguageCodes?.ToArray(),
            CreatePreview = request.PreviewCreation == null
                ? PreviewCreation.UseProjectPreference
                : (PreviewCreation)int.Parse(request.PreviewCreation),
            ExternalDocumentId = request.ExternalDocumentId
        };

        if (request.ImportEmbeddedImages != null) options.ImportEmbeddedImages = (bool)request.ImportEmbeddedImages;
        if (request.ImportEmbeddedObjects != null) options.ImportEmbeddedObjects = (bool)request.ImportEmbeddedObjects;
        if (request.FilterConfigResGuid != null)
        {
            options.FilterConfigResGuid = GuidExtensions.ParseWithErrorHandling(request.FilterConfigResGuid);
        }
        else if (request.File.Name.EndsWith(".xliff"))
        {
            file.Position = 0;
            var reader = new StreamReader(file);
            options.ImportSettingsXML = reader.ReadToEnd();
        }


        var results = await ExecuteWithHandling(() => ProjectService.Service.ImportTranslationDocumentsWithOptionsAsync(
            GuidExtensions.ParseWithErrorHandling(request.ProjectGuid),
            new List<ImportTranslationDocumentOptions> { options }.ToArray())
        );

        foreach (var result in results)
        {
            if (result.ResultStatus == ResultStatus.Error)
            {
                throw new PluginApplicationException(
                    $"Error while importing file, status: {result.ResultStatus}, message: {result.DetailedMessage}");
            }
        }

        return new()
        {
            DocumentGuids = results.SelectMany(x => x.DocumentGuids.Select(x => x.ToString()))
        };
    }

    [Action("Re-import file", Description = "Uploads and re-imports a file to a project")]
    public async Task<UploadFileResponse> UploadAndReimportFileToProject(
        [ActionParameter] UploadDocumentToProjectRequest request,
        [ActionParameter] ReimportDocumentsRequest reimportDocumentsRequest)
    {

        var fileStream = await fileManagementClient.DownloadAsync(request.File);
        var file = new MemoryStream();
        await fileStream.CopyToAsync(file);
        file.Position = 0;

        var fileBytes = await file.GetByteData();
        var uploadFileResult = FileUploader.UploadFile(fileBytes, FileUploadManager, request.File.Name);

        string? importSettings = null;
        if (request.File.Name.EndsWith(".xliff"))
        {
            file.Position = 0;
            importSettings = await new StreamReader(file).ReadToEndAsync();
        }

        var results = await ExecuteWithHandling(() => ProjectService.Service.ReImportTranslationDocumentsAsync(GuidExtensions.ParseWithErrorHandling(request.ProjectGuid),
            new[]
            {
                new ReimportDocumentOptions
                {
                    DocumentsToReplace = [GuidExtensions.ParseWithErrorHandling(reimportDocumentsRequest.DocumentGuid)],
                    FileGuid = uploadFileResult,
                    KeepUserAssignments = reimportDocumentsRequest.KeepUserAssignments ?? default,
                    PathToSetAsImportPath = reimportDocumentsRequest.PathToSetAsImportPath ?? string.Empty
                },
            }, importSettings));

        foreach (var result in results)
        {
            if (result.ResultStatus == ResultStatus.Error)
            {
                throw new PluginApplicationException(
                    $"Error while importing file, status: {result.ResultStatus}, message: {result.DetailedMessage}");
            }
        }

        return new()
        {
            DocumentGuids = results.SelectMany(x => x.DocumentGuids.Select(x => x.ToString()))
        };
    }

    [Action("Update file from XLIFF file", Description = "Update a project file by providing an XLIFF file")]
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

        var file = await fileManagementClient.DownloadAsync(request.File);
        byte[] fileBytes = await ProcessXliffFile(file, request.File.Name);

        var mqXliffFile = await fileManagementClient.DownloadAsync(mqXliffFileResponse.File);

        var updatedMqXliffFile = UpdateMqxliffFile(mqXliffFile, new MemoryStream(fileBytes),
            XliffRequest.UpdateLocked.GetValueOrDefault(true), XliffRequest.UpdateConfirmed.GetValueOrDefault(true));
        string mqXliffFileName = request.File.Name + ".mqxliff";

        updatedMqXliffFile.Position = 0;
        var bytes = await updatedMqXliffFile.GetByteData();

        var uploadFileResult =
            FileUploader.UploadFile(bytes, FileUploadManager, mqXliffFileName);

        var results = await ExecuteWithHandling(() => ProjectService.Service.UpdateTranslationDocumentFromBilingualAsync(
            GuidExtensions.ParseWithErrorHandling(request.ProjectGuid),
            uploadFileResult, BilingualDocFormat.XLIFF));

        foreach (var result in results)
        {
            if (result.ResultStatus == ResultStatus.Error)
            {
                throw new PluginApplicationException(
                    $"Error while importing file, status: {result.ResultStatus}, message: {result.DetailedMessage}");
            }
        }

        return new()
        {
            DocumentGuids = results.SelectMany(x => x.DocumentGuids.Select(x => x.ToString()))
        };
    }

    [Action("Upload XLIFF file", Description = "Uploads and imports an XLIFF file to a project")]
    public async Task<UploadFileResponse> UploadAndImportFileToProjectAsXliff(
        [ActionParameter] UploadDocumentToProjectRequest request,
        [ActionParameter] ImportDocumentAsXliffRequest importDocumentAsXliffRequest)
    {
        var file = await fileManagementClient.DownloadAsync(request.File);
        byte[] fileBytes = request.File.Name.EndsWith(".xliff")
            ? await ProcessXliffFile(file, request.File.Name)
            : await file.GetByteData();

        var fileName = request.File.Name;
        string fileReferenceName = string.Empty;
        if (fileName.EndsWith(".xliff"))
        {
            fileReferenceName = fileName.Replace(".xliff", "-2.1.xliff");
        }

        fileName = string.IsNullOrEmpty(fileReferenceName) ? fileName : fileReferenceName;
        var xliffFileReference =
            await fileManagementClient.UploadAsync(new MemoryStream(fileBytes), MediaTypeNames.Application.Xml,
                fileName);

        return string.IsNullOrEmpty(importDocumentAsXliffRequest.DocumentGuid)
            ? await UploadAndImportFileToProject(new UploadDocumentToProjectWithOptionsRequest
            {
                File = xliffFileReference, 
                ProjectGuid = request.ProjectGuid,
                TargetLanguageCodes = request.TargetLanguageCodes
            })
            : await ReimportDocumentAsync(importDocumentAsXliffRequest, xliffFileReference, request);
    }

    [Action("Download file", Description = "Exports and downloads a file with options")]
    public async Task<DownloadFileResponse> DownloadFileByGuid(
        [ActionParameter] DownloadFileRequest request)
    {
        var exportResult = await ExecuteWithHandling(() => ProjectService.Service
            .ExportTranslationDocument2Async(GuidExtensions.ParseWithErrorHandling(request.ProjectGuid), GuidExtensions.ParseWithErrorHandling(request.DocumentGuid),
                new DocumentExportOptions
                {
                    ExportAllMultilingualSiblings = request.ExportAllMultilingualSibling ?? true,
                    RevertFaultyTargetsToSource = request.RevertFaultyTargetsToSource ?? false,
                    CopySourceToEmptyTarget = request.CopySourceToEmptyTarget ?? false,
                    CopySourceToUnconfirmedRows = request.CopySourceToEmptyTarget ?? false,
                }));

        var data = DownloadFile(FileService.Service, exportResult.FileGuid, out var filename);
        var document = await GetFile(new()
        {
            ProjectGuid = request.ProjectGuid
        }, request.DocumentGuid);

        using var stream = new MemoryStream(data);
        var fileReference = await fileManagementClient.UploadAsync(stream, MimeTypes.GetMimeType(filename), filename);
        return new DownloadFileResponse(document) { File = fileReference };
    }

    [Action("Download XLIFF file",
        Description = "Exports and downloads the translation file as an XLIFF (MQXLIFF) bilingual")]
    public async Task<DownloadFileResponse> DownloadFileAsXliff(
        [ActionParameter] GetDocumentRequest documentRequest,
        [ActionParameter] DownloadXliffRequest request)
    {
        var fullVersion = request.FullVersionHistory ?? false;
        var includeSkeleton = request.IncludeSkeleton ?? false;

        var exportResult = await ExecuteWithHandling(() => ProjectService.Service
            .ExportTranslationDocumentAsXliffBilingualAsync(GuidExtensions.ParseWithErrorHandling(documentRequest.ProjectGuid),
                GuidExtensions.ParseWithErrorHandling(documentRequest.DocumentGuid), new XliffBilingualExportOptions
                {
                    FullVersionHistory = fullVersion,
                    IncludeSkeleton = includeSkeleton,
                    SaveCompressed = fullVersion || includeSkeleton || (request.SaveCompressed ?? false)
                }));

        var data = DownloadFile(FileService.Service, exportResult.FileGuid, out var filename);
        var document = await GetFile(new()
        {
            ProjectGuid = documentRequest.ProjectGuid
        }, documentRequest.DocumentGuid);

        using var stream = new MemoryStream(data);

        var useMqxliff = request.UseMqxliff ?? false;
        if (useMqxliff)
        {
            var fileReference = await fileManagementClient.UploadAsync(stream, "application/xliff+xml", filename);
            return new(document) { File = fileReference };
        }

        var fileReferenceXliff = await ConvertMqXliffToXliff(stream, filename.Replace(".mqxliff", ".xliff"));
        return new(document) { File = fileReferenceXliff };
    }

    [Action("Get file analysis", Description = "Get analysis for a specific file")]
    public async Task<GetAnalysisResponse> GetAnalysisForFile(
        [ActionParameter] GetAnalysisForFileRequest input)
    {
        var task = await ExecuteWithHandling(() => ProjectService.Service.StartStatisticsOnTranslationDocumentsTask2Async(
            new()
            {
                ProjectGuid = GuidExtensions.ParseWithErrorHandling(input.ProjectGuid),
                DocumentOrSliceGuids = new[] { GuidExtensions.ParseWithErrorHandling(input.DocumentGuid) },
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
            }));

        var taskResult = (StatisticsTaskResult) await WaitForAsyncTaskToFinishAndGetResult(task.TaskId);
        var document = await GetFile(input, input.DocumentGuid);
        var statistics = taskResult.ResultsForTargetLangs.First();

        return new(
            statistics,
            fileManagementClient,
            $"{Path.GetFileNameWithoutExtension(document.Name)}_{statistics.TargetLangCode}.html",
            MediaTypeNames.Text.Html);
    }

    [Action("Get project analysis", Description = "Get analysis for all project files")]
    public async Task<GetAnalysesForAllDocumentsResponse> GetAnalysesForAllDocuments(
        [ActionParameter] GetAnalysisForProjectRequest input)
    {
        var task = await ExecuteWithHandling(() => ProjectService.Service.StartStatisticsOnProjectTask2Async(new()
        {
            ProjectGuid = GuidExtensions.ParseWithErrorHandling(input.ProjectGuid),
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
        }));

        var taskResult = (StatisticsTaskResult) await WaitForAsyncTaskToFinishAndGetResult(task.TaskId);
        var analysis = taskResult.ResultsForTargetLangs
            .Select(x => new GetAnalysisResponse(x, fileManagementClient, $"Analysis_{x.TargetLangCode}.html",
                MediaTypeNames.Text.Html))
            .ToList();

        return new()
        {
            Analyses = analysis
        };
    }

    [Action("Apply translated content to updated source", Description = "Apply translated content to updated source")]
    public async Task<ApplyTranslatedContentToUpdatedSourceResponse> ApplyTranslatedContentToUpdatedSource(
        [ActionParameter] ApplyTranslatedContentToUpdatedSourceRequest input)
    {
        var taskInfo = await ExecuteWithHandling(() => ProjectService.Service.StartXTranslateTaskAsync(GuidExtensions.ParseWithErrorHandling(input.ProjectGuid),
            new()
            {
                XTranslateScenario = EnumParser.Parse<XTranslateScenario>(input.XTranslateScenario,
                    nameof(input.XTranslateScenario)) ?? default,

                WorkWithContextIds = input.WorkWithContextIds ?? default,
                DocInfos = new XTranslateDocInfo[]
                {
                    new()
                    {
                        DocumentGuid = GuidExtensions.ParseWithErrorHandling(input.DocumentGuid)
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
            }));

        var taskResult = (XTranslateTaskResult) await WaitForAsyncTaskToFinishAndGetResult(taskInfo.TaskId);
        var documents = taskResult.DocumentResults
            .Select(x => new XTranslateDocumentDto(x))
            .ToArray();

        return new()
        {
            Results = documents
        };
    }

    [Action("Deliver file", Description = "Deliver a specific file")]
    public async Task DeliverDocument([ActionParameter] ProjectRequest project,
        [ActionParameter] DeliverDocumentInput input)
    {
        var request = new DeliverDocumentRequest
        {
            DocumentGuid = GuidExtensions.ParseWithErrorHandling(input.DocumentGuid),
            DeliveringUserGuid = GuidExtensions.ParseWithErrorHandling(input.DeliveringUserGuid),
            ReturnDocToPreviousActor = input.ReturnDocToPreviousActorField ?? default,
        };

        await ExecuteWithHandling(() => ProjectService.Service.DeliverDocumentAsync(GuidExtensions.ParseWithErrorHandling(project.ProjectGuid), request));
    }

    private byte[] DownloadFile(IFileManagerService fmService, Guid fileGuid, out string fileName)
    {
        const int chunkSize = 500000;
        byte[] chunkBytes;

        try
        {
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
        } catch(Exception ex)
        {
            var root = ex;
            while (root.InnerException != null)
                root = root.InnerException;

            InvocationContext.Logger?.LogError.Invoke("Error while downloading memoQ file {FileGuid}. Exception: {Exception}. Root: {RootException}",
            new object[]
            {
                fileGuid,
                ex,
                root
            });

            throw new PluginApplicationException($"memoQ download failed: {root.Message}");
        }        
    }

    private async Task<TaskResult> WaitForAsyncTaskToFinishAndGetResult(Guid taskId)
    {
        var task = await ExecuteWithHandling(() => TaskService.Service.GetTaskStatusAsync(taskId));
        var i = 1;
        while (task.Status == MQS.TasksService.TaskStatus.Pending
               || task.Status == MQS.TasksService.TaskStatus.Executing)
        {
            if (i < 16)
                i = 2 * i;
            Thread.Sleep(i * 1000);
            task = await ExecuteWithHandling(() => TaskService.Service.GetTaskStatusAsync(taskId));
        }

        if (task.Status != MQS.TasksService.TaskStatus.Completed)
            throw new Exception($"Task has status {task.Status.ToString()}.");

        return await ExecuteWithHandling(() => TaskService.Service.GetTaskResultAsync(taskId));
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
        using (var xliffStream = new MemoryStream())
        {
            using (var writer = new StreamWriter(xliffStream))
            {
                writer.Write(xliffFile);
                writer.Flush();

                xliffStream.Position = 0;

                string contentType = MediaTypeNames.Text.Plain;

                return await fileManagementClient.UploadAsync(xliffStream, contentType, fileName);
            }
        }
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

        throw new PluginMisconfigurationException("Unsupported XLIFF version. Currently only 1.2 and 2.1 are supported.");
    }

    private async Task<UploadFileResponse> ReimportDocumentAsync(
        ImportDocumentAsXliffRequest importDocumentAsXliffRequest,
        FileReference fileReference,
        UploadDocumentToProjectRequest request)
    {
        if (importDocumentAsXliffRequest.UpdateSegmentStatuses != null &&
            importDocumentAsXliffRequest.UpdateSegmentStatuses.Value)
        {
            var mqXliffFileResponse = await DownloadFileAsXliff(
                new GetDocumentRequest
                {
                    ProjectGuid = request.ProjectGuid,
                    DocumentGuid = importDocumentAsXliffRequest.DocumentGuid ??
                                   throw new PluginMisconfigurationException("Can not reimport without file ID")
                },
                new DownloadXliffRequest
                {
                    FullVersionHistory = false,
                    UseMqxliff = true
                });

            var mqXliffFile = await fileManagementClient.DownloadAsync(mqXliffFileResponse.File);
            var fileStream = await fileManagementClient.DownloadAsync(fileReference);

            var updatedMqXliffFile = UpdateMqxliffFile(mqXliffFile, fileStream);
            string mqXliffFileName = request.File.Name + ".mqxliff";
            fileReference = await fileManagementClient.UploadAsync(updatedMqXliffFile, MediaTypeNames.Application.Xml,
                mqXliffFileName);
        }

        return await UploadAndReimportFileToProject(new UploadDocumentToProjectRequest
        {
            File = fileReference,
            ProjectGuid = request.ProjectGuid,
            TargetLanguageCodes = request.TargetLanguageCodes,
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

            if (mqTarget != null && xliffTargetNodes != null &&
                !mqTarget.Nodes().SequenceEqual(xliffTargetNodes, new XNodeEqualityComparer()))
            {
                if (!locked && mqTransUnit.Attribute(XNamespace.Get("MQXliff") + "locked")?.Value == "locked")
                {
                    continue;
                }

                if (!confirmed && mqTransUnit.Attribute(XNamespace.Get("MQXliff") + "status")?.Value == "Proofread")
                {
                    continue;
                }

                mqTarget.RemoveAll();
                mqTarget.Add(xliffTargetNodes);
                mqTransUnit.SetAttributeValue(XNamespace.Get("MQXliff") + "status", "Edited");
            }
        }

        var updatedString = Regex.Replace(mqXliffDoc.ToString(),
                "(<(source(.*?)|\\/bpt|\\/ph|target(.*?))>)\\r?\\n\\s+(?!\\s?(<target|<\\/trans-unit>))", "${1}");
        updatedString = Regex.Replace(updatedString,"&(?!(\\w+|#\\d+);)", "&amp;");
        var updatedMqXliffStream = new MemoryStream(Encoding.UTF8.GetBytes(updatedString));
        updatedMqXliffStream.Position = 0;
        return updatedMqXliffStream;
    }

    private async Task<Dictionary<string, string>> GetExistingDocumentGuidsByLanguageCode(
        string projectGuid,
        IEnumerable<string> targetLanguageCodes,
        string documentName,
        string? externalDocumentId = null)
    {
        var existingDocumentGuidsByLanguageCode = new Dictionary<string, string>();

        IEnumerable<ServerProjectTranslationDocInfo2> projectDocumentList = await ExecuteWithHandling(() => ProjectService.Service
            .ListProjectTranslationDocuments2Async(GuidExtensions.ParseWithErrorHandling(projectGuid), new()));

        projectDocumentList = projectDocumentList
            .Where(d => d.DocumentName == documentName)
            .Where(d => string.IsNullOrEmpty(externalDocumentId) || d.ExternalDocumentId == externalDocumentId);

        foreach (var targetLanguageCode in targetLanguageCodes)
        {
            var existingDocument = projectDocumentList
                .Where(d => d.TargetLangCode == targetLanguageCode)
                .FirstOrDefault();

            if (existingDocument != null)
            {
                existingDocumentGuidsByLanguageCode.Add(targetLanguageCode, existingDocument.DocumentGuid.ToString());
            }
        }

        return existingDocumentGuidsByLanguageCode;
    }
}