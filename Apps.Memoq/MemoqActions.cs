using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.Memoq.Models.Requests;
using Apps.Memoq.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using MQS.FileManager;
using MQS.ServerProject;

namespace Apps.Memoq;

[ActionList]
public class MemoqActions
{
    [Action("Create project", Description = "Create a new project in memoQ")]
    public CreateProjectResponse CreateProject(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] CreateProjectRequest request)
    {
        var newProject = new ServerProjectDesktopDocsCreateInfo
        {
            CreateOfflineTMTBCopies = false,
            DownloadPreview = true,
            DownloadSkeleton = true,
            EnableSplitJoin = true,
            EnableWebTrans = true,
            EnableCommunication = false,
            AllowOverlappingWorkflow = true,
            AllowPackageCreation = false,
            PreventDeliveryOnQAError = false,
            RecordVersionHistory = true,
            CreatorUser = ApplicationConstants.AdminGuid,
            Deadline = DateTime.Now,
            Name = request.ProjectName,
            SourceLanguageCode = request.SourceLangCode,
            TargetLanguageCodes = request.TargetLangCodes.ToArray(),
        };
        MemoqServiceFactory<IServerProjectService>? projectService = null;
        try
        {
            projectService = new MemoqServiceFactory<IServerProjectService>(ApplicationConstants.ProjectServiceUrl, authenticationCredentialsProviders);
            var guid = projectService.Service.CreateProject(newProject);
            var projectInfo = projectService.Service.GetProject(guid);
            return new CreateProjectResponse()
            {
                ProjectGuid = guid.ToString(),
                ProjectDeadline = projectInfo.Deadline.ToString("yyyy-MM-dd"),
                TargetLanguages = projectInfo.TargetLanguageCodes,
                SourceLanguage = projectInfo.SourceLanguageCode
            };
        }

        finally
        {
            projectService?.Dispose();
        }
    }

    [Action("Create project from template", Description = "Create a new project based on an existing memoQ project template")]
    public CreateProjectResponse CreateProjectBasedOnTemplate(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, [ActionParameter] CreateProjectTemplateRequest request)
    {
        using var projectService = new MemoqServiceFactory<IServerProjectService>(ApplicationConstants.ProjectServiceUrl, authenticationCredentialsProviders);

        var newProject = new TemplateBasedProjectCreateInfo
        {
            Client = "blackbird",
            Domain = "blackbird",
            Project = "blackbird",
            Subject = "blackbird",
            CreatorUser = ApplicationConstants.AdminGuid,
            Description = request.ProjectName,
            Name = request.ProjectName,
            SourceLanguageCode = request.SourceLangCode,
            TargetLanguageCodes = request.TargetLangCodes.ToArray(),
            TemplateGuid = Guid.Parse(request.TemplateGuid)
        };

        var result = projectService.Service.CreateProjectFromTemplate(newProject);
        var projectInfo = projectService.Service.GetProject(result.ProjectGuid);

        return new CreateProjectResponse
        {
            ProjectGuid = result.ProjectGuid.ToString(),
            ProjectDeadline = projectInfo.Deadline.ToString("yyyy-MM-dd"),
            TargetLanguages = projectInfo.TargetLanguageCodes,
            SourceLanguage = projectInfo.SourceLanguageCode
        };
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

    private Guid UploadFile(byte[] file, string fileName, IFileManagerService service)
    {
        Guid fileGuid = Guid.Empty;
        var fileStream = new MemoryStream(file);
        try
        {
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

            return fileGuid;
        }
        finally
        {
            fileStream.Close();
            if (fileGuid != Guid.Empty)
                service.EndChunkedFileUpload(fileGuid);
        }
    }
}