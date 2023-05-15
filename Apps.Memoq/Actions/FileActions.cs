using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using MQS.FileManager;
using MQS.ServerProject;
using Apps.Memoq.Models.ServerProjects.Requests;
using Apps.Memoq.Models.Files.Requests;
using Apps.Memoq.Models.Files.Responses;
using MQS.TasksService;

namespace Apps.Memoq.Actions
{
    [ActionList]
    public class FileActions
    {
        [Action("List all project files", Description = "List all project files")]
        public ListAllProjectFilesResponse ListAllProjectFiles(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] string projectGuid)
        {
            using var projectService = new MemoqServiceFactory<IServerProjectService>(ApplicationConstants.ProjectServiceUrl, authenticationCredentialsProviders);
            var result = projectService.Service.ListProjectTranslationDocuments(Guid.Parse(projectGuid));

            return new ListAllProjectFilesResponse
            {
                Files = result
            };
        }

        [Action("Get file info", Description = "Get project file info by guid")]
        public ServerProjectTranslationDocInfo? GetFile(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] string projectGuid, [ActionParameter] string fileGuid)
        {
            var files = ListAllProjectFiles(authenticationCredentialsProviders, projectGuid);
            return files.Files.FirstOrDefault(f => f.DocumentGuid == Guid.Parse(fileGuid));
        }

        [Action("Assign file to user", Description = "Assign file to user")]
        public void AssignFileToUser(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] AssignFileToUserRequest input)
        {
            using var projectService = new MemoqServiceFactory<IServerProjectService>(ApplicationConstants.ProjectServiceUrl, authenticationCredentialsProviders);
            var assignments = new ServerProjectTranslationDocumentUserAssignments[] { 
                new ServerProjectTranslationDocumentUserAssignments()
                {
                    DocumentGuid = Guid.Parse(input.FileGuid),
                    UserRoleAssignments = new TranslationDocumentUserRoleAssignment[] { 
                        new TranslationDocumentUserRoleAssignment() { 
                            UserGuid = Guid.Parse(input.UserGuid),
                            DeadLine = DateTime.Parse(input.Deadline)
                        } 
                    }
                }
            };
            projectService.Service.SetProjectTranslationDocumentUserAssignments(Guid.Parse(input.ProjectGuid), assignments);
        }

        [Action("Upload file to project", Description = "Uploads and imports a file to a project")]
        public UploadFileResponse UploadAndImportFileToProject(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] UploadDocumentToProjectRequest request)
        {
            using var fileService = new MemoqServiceFactory<IFileManagerService>(ApplicationConstants.FileServiceUrl, authenticationCredentialsProviders);
            using var projectService = new MemoqServiceFactory<IServerProjectService>(ApplicationConstants.ProjectServiceUrl, authenticationCredentialsProviders);
            var uploadFileResult = UploadFile(request.File, request.FileName, fileService.Service);
            var result = projectService.Service.ImportTranslationDocument(Guid.Parse(request.ProjectGuid), uploadFileResult, null, null);
            return new UploadFileResponse
            {
                DocumentGuids = result.DocumentGuids.Select(x => x.ToString()).ToArray()
            };
        }

        [Action("Download file by guid", Description = "Download file by guid")]
        public DownloadFileResponse DownloadFileByGuid(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] DownloadFileRequest request)
        {
            using var fileService = new MemoqServiceFactory<IFileManagerService>(ApplicationConstants.FileServiceUrl, authenticationCredentialsProviders);
            using var projectService = new MemoqServiceFactory<IServerProjectService>(ApplicationConstants.ProjectServiceUrl, authenticationCredentialsProviders);

            var exportResult = projectService.Service.ExportTranslationDocument(Guid.Parse(request.ProjectGuid), Guid.Parse(request.FileGuid));
            string filename = "";
            var data = DownloadFile(fileService.Service, exportResult.FileGuid, out filename);
            return new DownloadFileResponse
            {
                FileName = filename,
                File = data
            };
        }

        [Action("Get analysis for a file", Description = "Get analysis for a file")]
        public GetAnalysisForFileResponse GetAnalysisForFile(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetAnalysisForFileRequest input)
        {
            using var projectService = new MemoqServiceFactory<IServerProjectService>(ApplicationConstants.ProjectServiceUrl, authenticationCredentialsProviders);
            var task = projectService.Service.StartStatisticsOnTranslationDocumentsTask2(new CreateStatisticsOnDocumentsRequest()
            {
                ProjectGuid = Guid.Parse(input.ProjectGuid),
                DocumentOrSliceGuids = new Guid[] { Guid.Parse(input.DocumentGuid) }
            });
            var taskResult = (StatisticsTaskResult)WaitForAsyncTaskToFinishAndGetResult(authenticationCredentialsProviders, task.TaskId);
            var result = projectService.Service.GetAnalysisReportAsCSV(Guid.Parse(input.ProjectGuid), taskResult.AnalysisReportId ?? Guid.Empty);
            return new GetAnalysisForFileResponse
            {
                Analyses = result.DataForTargetLangs.ToList()
            };
        }

        private Guid UploadFile(byte[] file, string fileName, IFileManagerService service)
        {
            using var fileStream = new MemoryStream(file);
            const int chunkSize = 500000;
            byte[] chunkBytes = new byte[chunkSize];
            int bytesRead;
            Guid fileGuid = service.BeginChunkedFileUpload(fileName, false);
            while ((bytesRead = fileStream.Read(chunkBytes, 0, chunkSize)) != 0)
            {
                byte[] dataToUpload;
                if (bytesRead == chunkSize)
                    dataToUpload = chunkBytes;
                else
                {
                    dataToUpload = new byte[bytesRead];
                    Array.Copy(chunkBytes, dataToUpload, bytesRead);
                }
                service.AddNextFileChunk(fileGuid, dataToUpload);
            }
            if (fileGuid != Guid.Empty)
                service.EndChunkedFileUpload(fileGuid);
            return fileGuid;
        }

        private byte[] DownloadFile(IFileManagerService fmService, Guid fileGuid, out string fileName)
        {
            const int chunkSize = 500000;
            byte[] chunkBytes;

            var downloadResponse = fmService.BeginChunkedFileDownload(new BeginChunkedFileDownloadRequest(fileGuid, false));
            fileName = downloadResponse.fileName;
            int fileBytesLeft = downloadResponse.fileSize;
            byte[] result = new byte[fileBytesLeft];

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

        private TaskResult WaitForAsyncTaskToFinishAndGetResult(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, Guid TaskId)
        {
            using var taskService = new MemoqServiceFactory<ITasksService>(ApplicationConstants.TaskServiceUrl, authenticationCredentialsProviders);
            var taskStatus = taskService.Service.GetTaskStatus(TaskId).Status;
            int i = 1;
            while (taskStatus != MQS.TasksService.TaskStatus.Cancelled
                        && taskStatus != MQS.TasksService.TaskStatus.Completed
                        && taskStatus != MQS.TasksService.TaskStatus.Failed
                        && taskStatus != MQS.TasksService.TaskStatus.InvalidTask)
            {
                if (i < 16)
                    i = 2 * i;
                Thread.Sleep(i * 1000);
                taskStatus = taskService.Service.GetTaskStatus(TaskId).Status;
            }
            if (taskStatus != MQS.TasksService.TaskStatus.Completed)
                throw new Exception($"Task has status {taskStatus.ToString()}.");
            return taskService.Service.GetTaskResult(TaskId);
        }
    }
}
