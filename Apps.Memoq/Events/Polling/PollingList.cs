using Apps.Memoq.Contracts;
using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Apps.Memoq.Events.Polling.Models.Memory;
using Apps.Memoq.Models;
using Apps.Memoq.Models.Dto;
using Apps.Memoq.Models.ServerProjects.Requests;
using Apps.Memoq.Models.ServerProjects.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using MQS.ServerProject;
using Apps.MemoQ;
using Apps.MemoQ.Extensions;
using MQS.TasksService;
using Apps.MemoQ.Events.Polling.Models;

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

    private Task<ServerProjectInfo[]> ListProjects(MemoqServiceFactory<IServerProjectService> projectService) =>
        projectService.Service.ListProjectsAsync(new());

    private Task<ServerProjectInfo> GetProject(MemoqServiceFactory<IServerProjectService> projectService,
        string projectId) => projectService.Service.GetProjectAsync(GuidExtensions.ParseWithErrorHandling(projectId));
}