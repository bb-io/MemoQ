﻿using System.Net;
using System.Text;
using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.Memoq.Models.Requests;
using Apps.Memoq.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using MQS.FileManager;
using MQS.ServerProject;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Apps.Memoq;

[ActionList]
public class MemoqActions
{
    [Action]
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
            SourceLanguageCode = request.SourseLangCode,
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

    [Action]
    public CreateProjectResponse CreateProjectBasedOnTemplate(string url,
        AuthenticationCredentialsProvider authenticationCredentialsProvider,
        [ActionParameter] CreateProjectTemplateRequest request)
    {
        const string clientMeta = "blackbird";
        using var projectService = new MemoqServiceFactory<IServerProjectService>(
            $"{url}{ApplicationConstants.ProjectServiceUrl}", authenticationCredentialsProvider.Value);
        var newProject = new TemplateBasedProjectCreateInfo
        {
            Client = clientMeta,
            Domain = clientMeta,
            Project = clientMeta,
            Subject = clientMeta,
            CreatorUser = ApplicationConstants.AdminGuid,
            Description = request.ProjectName,
            Name = request.ProjectName,
            SourceLanguageCode = request.SourseLangCode,
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


    [Action]
    public UploadFileResponse UploadAndImportFileToProject(string url,
        AuthenticationCredentialsProvider authenticationCredentialsProvider,
        [ActionParameter] UploadDocumentToProjectRequest request)
    {
        var uploadFileResult =
            FileUploadFromAzureToMemoq(url, authenticationCredentialsProvider.Value, request.FilePath);
        return ImportFileToProject(url, authenticationCredentialsProvider, new ImportDocumentRequest
        {
            ProjectGuid = request.ProjectGuid,
            FileGuid = uploadFileResult.ToString()
        });
    }


    public GenerateFileResponse GenerateFilesFromHubspotPosts(string url, string apiKey,
        [ActionParameter] GenerateFileRequest request)
    {
        var json = JObject.Parse(request.FileContent.ToString()).SelectToken("results");
        var files = new List<string>();
        foreach (var jToken in json)
        {
            var path = UploadFileToBlob(JsonConvert.SerializeObject(jToken));
            files.Add(path);
        }

        return new GenerateFileResponse
        {
            StatusCode = HttpStatusCode.OK,
            Files = files.ToArray(),
            Total = files.Count
        };
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

    private Guid FileUploadFromAzureToMemoq(string baseServiceUrl, string apiKey, string sourceFilePath)
    {
        var storageAccount = CloudStorageAccount.Parse(ApplicationConstants.AzureBlobConnectionString);
        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        CloudBlobContainer container = blobClient.GetContainerReference(ApplicationConstants.AzureBlobContainer);
        CloudBlockBlob blockBlob = container.GetBlockBlobReference(sourceFilePath);
        blockBlob.FetchAttributes();
        var contentType = blockBlob.Properties.ContentType;
        var provider = new FileExtensionContentTypeProvider();
        var fileExt = provider.Mappings.FirstOrDefault(x => x.Value == contentType).Key;
        using var stream = blockBlob.OpenRead();
        var result = UploadFileToMemoq(stream, $"{sourceFilePath}{fileExt}", baseServiceUrl, apiKey);
        return result;
    }

    private string UploadFileToBlob(string fileContent)
    {
        var storageAccount = CloudStorageAccount.Parse(ApplicationConstants.AzureBlobConnectionString);
        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        CloudBlobContainer container = blobClient.GetContainerReference(ApplicationConstants.AzureBlobContainer);
        string fsName = $"storage/{Guid.NewGuid()}";
        CloudBlockBlob blockBlob = container.GetBlockBlobReference(fsName);
        blockBlob.Properties.ContentType = "application/json";
        using MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
        blockBlob.UploadFromStream(ms);
        return fsName;
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