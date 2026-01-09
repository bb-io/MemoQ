using Apps.Memoq.Contracts;
using Apps.Memoq.Events.Polling.Models.Memory;
using Apps.Memoq.Models;
using Apps.Memoq.Models.Dto;
using Apps.Memoq.Models.ServerProjects.Requests;
using Apps.Memoq.Models.ServerProjects.Responses;
using Apps.MemoQ;
using Apps.MemoQ.Callbacks.Models.Response;
using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Apps.MemoQ.Events.Polling.Models;
using Apps.MemoQ.Events.Polling.Models.Memory;
using Apps.MemoQ.Extensions;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using MQS.ServerProject;
using MQS.TasksService;

namespace Apps.Memoq.Events.Polling;

[PollingEventList]
public class PollingList : MemoqInvocable
{
    public PollingList(InvocationContext invocationContext) : base(invocationContext)
    {
    }


    [PollingEvent("On task status changed", "Triggers when the status of a specific pretranslation task changes")]
    public async Task<PollingEventResponse<TaskStatusMemory, TaskStatusResponse>> OnTaskStatusChanged(
            PollingEventRequest<TaskStatusMemory> request,
            [PollingEventParameter][Display("Task ID")] string taskId,
            [PollingEventParameter] [Display("Task status")] [StaticDataSource(typeof(TaskStatusDataHandler))]
            string? targetTaskStatus)
    {
        var taskGuid = GuidExtensions.ParseWithErrorHandling(taskId);

        var taskInfo = await ExecuteWithHandling(() => TaskService.Service.GetTaskStatusAsync(taskGuid));

        if (request.Memory is null)
        {
            return new()
            {
                FlyBird = false,
                Memory = new TaskStatusMemory
                {
                    Status = "Pending",
                    LastCheckDate = DateTime.UtcNow
                }
            };
        }

        var currentStatus = taskInfo.Status.ToString();
        var previousStatus = request.Memory.Status;

        if (currentStatus == previousStatus ||
            (!string.IsNullOrEmpty(targetTaskStatus) && currentStatus != targetTaskStatus))
        {
            return new()
            {
                FlyBird = false,
                Memory = new TaskStatusMemory
                {
                    Status = currentStatus,
                    LastCheckDate = DateTime.UtcNow
                }
            };
        }

        return new()
        {
            FlyBird = true,
            Result = new TaskStatusResponse
            {
                TaskId = taskInfo.TaskId.ToString(),
                TaskStatus = taskInfo.Status.ToString(),
                ProgressPercentage = taskInfo.ProgressPercentage
            },
            Memory = new TaskStatusMemory
            {
                Status = currentStatus,
                LastCheckDate = DateTime.UtcNow
            }
        };
    }


    [PollingEvent("On projects created", "On new projects are created")]
    public async Task<PollingEventResponse<EntityCreatedMemory, ListAllProjectsResponse>> OnProjectCreated(
        PollingEventRequest<EntityCreatedMemory> request)
    {
        if (request.Memory is null)
        {
            return new()
            {
                FlyBird = false,
                Memory = new()
                {
                    LastCreationDate = DateTime.UtcNow
                }
            };
        }

        var items =
            (await ListProjects(ProjectService))
            .Where(x => x.CreationTime > request.Memory.LastCreationDate)
            .ToArray();

        if (!items.Any())
        {
            return new()
            {
                FlyBird = false,
                Memory = new()
                {
                    LastCreationDate = DateTime.UtcNow
                }
            };
        }

        return new()
        {
            FlyBird = true,
            Result = new()
            {
                ServerProjects = items.Select(x => new ProjectDto(x)).ToList()
            },
            Memory = new()
            {
                LastCreationDate = DateTime.UtcNow
            }
        };
    }

    [PollingEvent("On project status changed", "On status of a specific project changed")]
    public async Task<PollingEventResponse<EntityChangedMemory, ProjectDto>> OnProjectStatusChanged(
        PollingEventRequest<EntityChangedMemory> request,
        [PollingEventParameter] ProjectRequest projectRequest,
        [PollingEventParameter] [Display("Project status")] [StaticDataSource(typeof(ProjectStatusDataHandler))]
        string? projectStatus)
    {
        var project = new ProjectDto(await GetProject(ProjectService, projectRequest.ProjectGuid));

        if (request.Memory is null)
        {
            return new()
            {
                FlyBird = false,
                Memory = new()
                {
                    Status = project.Status,
                    LastModificationDate = DateTime.UtcNow
                }
            };
        }

        if (project.Status == request.Memory.Status ||
            (!string.IsNullOrEmpty(projectStatus) && project.Status != projectStatus))
        {
            return new()
            {
                FlyBird = false,
                Memory = new()
                {
                    Status = project.Status,
                    LastModificationDate = DateTime.UtcNow
                }
            };
        }

        return new()
        {
            FlyBird = true,
            Result = project,
            Memory = new()
            {
                Status = project.Status,
                LastModificationDate = DateTime.UtcNow,
            }
        };
    }

    [PollingEvent("On all files status reached", "Triggers when ALL files of the specified project reached the specified status")]
    public async Task<PollingEventResponse<AllFilesDeliveredMemory, AllFilesDeliveredResponse>> OnAllFilesDelivered(
    PollingEventRequest<AllFilesDeliveredMemory> request,
    [PollingEventParameter] ProjectRequest projectRequest,
    [PollingEventParameter] TranslationFileStatus status)
    {
        if (string.IsNullOrWhiteSpace(status?.Status))
            throw new PluginMisconfigurationException("Status is required.");

        if (!Enum.TryParse<DocumentStatus>(status.Status, ignoreCase: true, out var targetStatus))
            throw new PluginMisconfigurationException($"Unknown status '{status.Status}'.");

        var projectGuid = GuidExtensions.ParseWithErrorHandling(projectRequest.ProjectGuid);

        var documents = await ExecuteWithHandling(() =>
            ProjectService.Service.ListProjectTranslationDocuments2Async(
                projectGuid,
                new() { FillInAssignmentInformation = false }));

        if (documents.Length == 0)
            return new() { FlyBird = false };

        var allReached = documents.All(d => d.DocumentStatus == targetStatus);
        if (!allReached)
            return new() { FlyBird = false };

        var result = new AllFilesDeliveredResponse
        {
            ProjectId = projectRequest.ProjectGuid,
            RequestedStatus = targetStatus.ToString(),
            Documents = documents.Select(d => new AllFilesDeliveredDocumentDto
            {
                Id = d.DocumentGuid.ToString(),
                Name = d.DocumentName,
                TargetLanguage = d.TargetLangCode,
                Status = d.DocumentStatus.ToString()
            }).ToList()
        };

        return new()
        {
            FlyBird = true,
            Result = result
        };
    }

    private Task<ServerProjectInfo[]> ListProjects(MemoqServiceFactory<IServerProjectService> projectService) =>
        projectService.Service.ListProjectsAsync(new());

    private Task<ServerProjectInfo> GetProject(MemoqServiceFactory<IServerProjectService> projectService,
        string projectId) => projectService.Service.GetProjectAsync(GuidExtensions.ParseWithErrorHandling(projectId));
}