using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.Memoq.Models.Requests;
using Apps.Memoq.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using MQS.FileManager;
using MQS.ServerProject;

namespace Apps.Memoq;

[ActionList]
public class MemoqActions
{
    [Action("Create project", Description = "Create a new project in memoQ")]
    public CreateProjectResponse CreateProject(string url,
        AuthenticationCredentialsProvider authenticationCredentialsProvider,
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
            projectService = new MemoqServiceFactory<IServerProjectService>(
                $"{url}{ApplicationConstants.ProjectServiceUrl}",
                authenticationCredentialsProvider.Value);
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
    public CreateProjectResponse CreateProjectBasedOnTemplate(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider, [ActionParameter] CreateProjectTemplateRequest request)
    {
        using var projectService = new MemoqServiceFactory<IServerProjectService>(
            $"{url}{ApplicationConstants.ProjectServiceUrl}", authenticationCredentialsProvider.Value);

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
    public UploadFileResponse UploadAndImportFileToProject(string url,
        AuthenticationCredentialsProvider authenticationCredentialsProvider,
        [ActionParameter] UploadDocumentToProjectRequest request)
    {
        var uploadFileResult =
            FileUpload(url, authenticationCredentialsProvider.Value, request.FilePath);
        return ImportFileToProject(url, authenticationCredentialsProvider, new ImportDocumentRequest
        {
            ProjectGuid = request.ProjectGuid,
            FileGuid = uploadFileResult.ToString()
        });
    }

    private UploadFileResponse ImportFileToProject(string url,
        AuthenticationCredentialsProvider authenticationCredentialsProvider, ImportDocumentRequest request)
    {
        using var memoqFileService =
            new MemoqServiceFactory<IServerProjectService>($"{url}{ApplicationConstants.ProjectServiceUrl}",
                authenticationCredentialsProvider.Value);
        var result = memoqFileService.Service.ImportTranslationDocument(Guid.Parse(request.ProjectGuid),
            Guid.Parse(request.FileGuid), null, null);
        return new UploadFileResponse
        {
            DocumentGuids = result.DocumentGuids.Select(x => x.ToString()).ToArray()
        };
    }

    private Guid FileUpload(string baseServiceUrl, string apiKey, string sourceFilePath)
    {
        var fileName = new FileInfo(sourceFilePath).Name;
        using var stream = File.OpenRead(sourceFilePath);
        var result = UploadFileToMemoq(stream, fileName, baseServiceUrl, apiKey);
        return result;
    }

    private Guid UploadFileToMemoq(Stream fileStream, string filePath, string baseServiceUrl, string apiKey)
    {
        Guid fileGuid = Guid.Empty;
        using var memoqFileService =
            new MemoqServiceFactory<IFileManagerService>($"{baseServiceUrl}{ApplicationConstants.FileServiceUrl}",
                apiKey);
        try
        {
            const int chunkSize = 500000;
            byte[] chunkBytes = new byte[chunkSize];
            int bytesRead;
            fileGuid = memoqFileService.Service.BeginChunkedFileUpload(filePath, false);
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

                memoqFileService.Service.AddNextFileChunk(fileGuid, dataToUpload);
            }

            return fileGuid;
        }
        finally
        {
            fileStream.Close();
            if (fileGuid != Guid.Empty)
                memoqFileService.Service.EndChunkedFileUpload(fileGuid);
        }
    }
}