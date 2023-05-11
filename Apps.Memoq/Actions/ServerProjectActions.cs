using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.Memoq.Models.Requests;
using Apps.Memoq.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using MQS.FileManager;
using MQS.ServerProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memoq.Actions
{
    [ActionList]
    public class ServerProjectActions
    {
        [Action("Create project", Description = "Create a new project")]
        public ServerProjectInfo CreateProject(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider,
        [ActionParameter] CreateProjectRequest request)
        {
            var newProject = new ServerProjectDesktopDocsCreateInfo
            {
                Deadline = DateTime.Parse(request.Deadline),
                Name = request.ProjectName,
                CreatorUser = ApplicationConstants.AdminGuid,
                SourceLanguageCode = request.SourceLangCode,
                TargetLanguageCodes = request.TargetLangCodes.ToArray(),
            };
            var projectService = new MemoqServiceFactory<IServerProjectService>(ApplicationConstants.ProjectServiceUrl, authenticationCredentialsProvider);
            var guid = projectService.Service.CreateProject(newProject);
            return projectService.Service.GetProject(guid);
        }

        [Action("Create project from template", Description = "Create project from template")]
        public ServerProjectInfo CreateProjectFromTemplate(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider,
        [ActionParameter] CreateProjectTemplateRequest request)
        {
            var newProject = new TemplateBasedProjectCreateInfo
            {
                Name = request.ProjectName,
                CreatorUser = ApplicationConstants.AdminGuid,
                SourceLanguageCode = request.SourceLangCode,
                TargetLanguageCodes = request.TargetLangCodes.ToArray(),
                TemplateGuid = Guid.Parse(request.TemplateGuid)
            };
            using var projectService = new MemoqServiceFactory<IServerProjectService>(ApplicationConstants.ProjectServiceUrl, authenticationCredentialsProvider);
            var result = projectService.Service.CreateProjectFromTemplate(newProject);
            return projectService.Service.GetProject(result.ProjectGuid);  
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

        private Guid UploadFile(byte[] file, string fileName, IFileManagerService service)
        {
            Guid fileGuid = Guid.Empty;
            using var fileStream = new MemoryStream(file);
            const int chunkSize = 500000;
            byte[] chunkBytes = new byte[chunkSize];
            int bytesRead;
            fileGuid = service.BeginChunkedFileUpload(fileName, false);
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
            return result;
        }
    }
}
