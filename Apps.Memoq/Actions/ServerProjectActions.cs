using Apps.Memoq.Contracts;
using Apps.Memoq.DataSourceHandlers;
using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Apps.Memoq.Extensions;
using Apps.Memoq.Models;
using Apps.Memoq.Models.Dto;
using Apps.Memoq.Models.Files.Requests;
using Apps.Memoq.Models.Files.Responses;
using Apps.Memoq.Models.ServerProjects.Requests;
using Apps.Memoq.Models.ServerProjects.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using MQS.ServerProject;
using ResourceType = MQS.ServerProject.ResourceType;

namespace Apps.Memoq.Actions;

[ActionList]
public class ServerProjectActions : BaseInvocable
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public ServerProjectActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("List projects", Description = "List all projects")]
    public ListAllProjectsResponse ListAllProjects([ActionParameter] ListProjectsRequest input)
    {
        var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        var response = projectService.Service.ListProjects(new()
        {
            Client = input.Client,
            Domain = input.Domain,
            LastChangedBefore = input.LastChangedBefore,
            NameOrDescription = input.NameOrDescription,
            Project = input.Project,
            SourceLanguageCode = input.SourceLanguageCode,
            TargetLanguageCode = input.TargetLanguageCode,
            Subject = input.Subject,
            TimeClosed = input.TimeClosed ?? default,
        });

        var projects = response.Select(x => new ProjectDto(x)).ToList();

        return new()
        {
            ServerProjects = projects
        };
    }

    [Action("Get project", Description = "Get project by UId")]
    public ProjectDto GetProject([ActionParameter] ProjectRequest project)
    {
        var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        var response = projectService.Service.GetProject(Guid.Parse(project.ProjectGuid));
        return new(response);
    }

    [Action("Add target language to project", Description = "Add target language to project by code")]
    public void AddNewTargetLanguageToProject(
        [ActionParameter] ProjectRequest project,
        [ActionParameter, StaticDataSource(typeof(TargetLanguageDataHandler)), Display("Target language")] string targetLangCode)
    {
        var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        projectService.Service
            .AddLanguageToProject(Guid.Parse(project.ProjectGuid), new()
            {
                TargetLangCode = targetLangCode
            });
    }

    [Action("Create a project", Description = "Creates a new project in memoQ")]
    public ProjectDto CreateProject([ActionParameter] CreateProjectRequest request)
    {
        using var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        var newProject = new ServerProjectDesktopDocsCreateInfo
        {
            Deadline = request.Deadline,
            Name = request.ProjectName,
            CreatorUser = SoapConstants.AdminGuid,
            SourceLanguageCode = request.SourceLangCode,
            TargetLanguageCodes = request.TargetLangCodes.ToArray(),
            CallbackWebServiceUrl = request.CallbackUrl ??
                                    $"{InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}{ApplicationConstants.MemoqBridgePath}"
                                        .SetQueryParameter("id",
                                            Creds.GetInstanceUrlHash()),
            Description = request.Description,
            Domain = request.Domain,
            Subject = request.Subject,
            StrictSubLangMatching = request.StrictSubLangMatching ?? default,
            EnableCommunication = request.EnableCommunication ?? default,
            DownloadSkeleton2 = request.DownloadSkeleton2 ?? default,
            DownloadPreview2 = request.DownloadPreview2 ?? default,
            CustomMetas = request.CustomMetas,
            Client = request.Client,
            AllowPackageCreation = request.AllowPackageCreation ?? default,
            AllowOverlappingWorkflow = request.AllowOverlappingWorkflow ?? default,
            EnableWebTrans = request.EnableWebTrans ?? default,
            EnableSplitJoin = request.EnableSplitJoin ?? default,
            DownloadSkeleton = request.DownloadSkeleton ?? default,
            DownloadPreview = request.DownloadPreview ?? default,
            CreateOfflineTMTBCopies = request.CreateOfflineTmtbCopies ?? default,
            ConfidentialitySettings = new()
            {
                DisableTMPlugins = request.DisableTmPlugins,
                DisableTBPlugins = request.DisableTbPlugins,
                DisableMTPlugins = request.DisableMtPlugins,
            }
        };

        var guid = projectService.Service.CreateProject2(newProject);
        var response = projectService.Service.GetProject(guid);

        return new(response);
    }

    [Action("Create a project from a template",
        Description = "Creates a new project based on an existing memoQ project template")]
    public ProjectDto CreateProjectFromTemplate([ActionParameter] CreateProjectTemplateRequest request)
    {
        using var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        var newProject = new TemplateBasedProjectCreateInfo
        {
            Name = request.ProjectName,
            CreatorUser = SoapConstants.AdminGuid,
            SourceLanguageCode = request.SourceLangCode,
            TargetLanguageCodes = request.TargetLangCodes.ToArray(),
            Description = request.Description,
            Domain = request.Domain,
            Subject = request.Subject,
            Client = request.Client,
            TemplateGuid = Guid.Parse(request.TemplateGuid)
        };

        var result = projectService.Service.CreateProjectFromTemplate(newProject);
        var response = projectService.Service.GetProject(result.ProjectGuid);

        return new(response);
    }

    [Action("Create a project from a package",
        Description = "Creates a new project based on an existing memoQ package")]
    public ProjectDto CreateProjectPackage([ActionParameter] CreateProjectFromPackageRequest input)
    {
        using var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        var request = new ServerProjectDesktopDocsCreateInfo
        {
            Deadline = input.Deadline,
            Name = input.ProjectName,
            CreatorUser = SoapConstants.AdminGuid,
            SourceLanguageCode = input.SourceLangCode,
            TargetLanguageCodes = input.TargetLangCodes.ToArray(),
            CallbackWebServiceUrl =
                input.CallbackUrl ??
                $"{InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}{ApplicationConstants.MemoqBridgePath}"
                    .SetQueryParameter("id",
                        Creds.GetInstanceUrlHash()),
            Description = input.Description,
            Domain = input.Domain,
            Subject = input.Subject,
            StrictSubLangMatching = input.StrictSubLangMatching ?? default,
            EnableCommunication = input.EnableCommunication ?? default,
            DownloadSkeleton2 = input.DownloadSkeleton2 ?? default,
            DownloadPreview2 = input.DownloadPreview2 ?? default,
            CustomMetas = input.CustomMetas,
            Client = input.Client,
            AllowPackageCreation = input.AllowPackageCreation ?? default,
            AllowOverlappingWorkflow = input.AllowOverlappingWorkflow ?? default,
            EnableWebTrans = input.EnableWebTrans ?? default,
            EnableSplitJoin = input.EnableSplitJoin ?? default,
            DownloadSkeleton = input.DownloadSkeleton ?? default,
            DownloadPreview = input.DownloadPreview ?? default,
            CreateOfflineTMTBCopies = input.CreateOfflineTmtbCopies ?? default,
            ConfidentialitySettings = new()
            {
                DisableTMPlugins = input.DisableTmPlugins,
                DisableTBPlugins = input.DisableTbPlugins,
                DisableMTPlugins = input.DisableMtPlugins,
            }
        };

        var importOptions = new PackageImportOptions
        {
            ImportResources = input.ImportResources
        };

        var result = projectService.Service.CreateProjectFromPackage3(request, Guid.Parse(input.FileId), importOptions);
        var response = projectService.Service.GetProject(result);

        return new(response);
    }

    [Action("Delete project", Description = "Delete a specific project")]
    public void DeleteProject([ActionParameter] ProjectRequest project)
    {
        var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        projectService.Service.DeleteProject(Guid.Parse(project.ProjectGuid));
    }

    [Action("Distribute project", Description = "Distribute a specific project")]
    public async Task DistributeProject([ActionParameter] ProjectRequest project)
    {
        var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        await projectService.Service.DistributeProjectAsync(Guid.Parse(project.ProjectGuid));
    }

    [Action("Add resource to project",
        Description = "Add resource to a specific project by type and ID, optionally with object IDs")]
    public async Task AddResourceToProject([ActionParameter] ProjectRequest project,
        [ActionParameter] AddResourceToProjectRequest request, [ActionParameter][Display("Overwrite?", Description = "Whether to overwrite the current resources, default to false")] bool? overwrite)
    {
        var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        var projectId = Guid.Parse(project.ProjectGuid);
        var resourceType = (ResourceType)int.Parse(request.ResourceType);
        var assignments = CreateAssignmentsBasedOnResourceType(resourceType, request);

        if (overwrite != true)
        {
            var currentResources = await projectService.Service.ListProjectResourceAssignmentsAsync(projectId, resourceType);
            foreach ( var currentResource in currentResources)
            {
                assignments.Add(new ServerProjectResourceAssignment
                {
                    ResourceGuid = currentResource.ResourceInfo.Guid,
                    ObjectId = currentResource.ObjectId,
                    Primary = currentResource.Primary
                });
            }
        }       
        
        var array = new[]
        {
            new ServerProjectResourceAssignmentForResourceType
            {
                ResourceType = resourceType,
                ServerProjectResourceAssignment = assignments.ToArray()
            }
        };

        await projectService.Service.SetProjectResourceAssignmentsAsync(projectId, array);
    }

    [Action("Pretranslate documents", Description = "Pretranslate documents if document GUIDs are provided, otherwise pretranslate the whole project with all documents")]
    public async Task<PretranslateDocumentsResponse> PretranslateDocuments(
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] PretranslateDocumentsRequest request)
    {
        var projectService = new MemoqServiceFactory<IServerProjectService>(
            SoapConstants.ProjectServiceUrl, Creds);

        var options = new PretranslateOptions
        {
            OnlyUnambiguousMatches = request.OnlyUnambiguousMatches ?? true,
            LockPretranslated = request.LockPretranslated ?? true,
            UseMT = request.UseMt ?? true,
            ConfirmLockPretranslated = request.ConfirmLockPreTranslated != null 
                ? (PretranslateStateToConfirmAndLock)int.Parse(request.ConfirmLockPreTranslated) 
                : PretranslateStateToConfirmAndLock.ExactMatch,
            FinalTranslationState = request.FinalTranslationState != null 
                ? (PretranslateExpectedFinalTranslationState)int.Parse(request.FinalTranslationState) 
                : PretranslateExpectedFinalTranslationState.NoChange
        };
        
        if (request.PretranslateLookupBehavior != null)
            options.PretranslateLookupBehavior =
                (PretranslateLookupBehavior)int.Parse(request.PretranslateLookupBehavior);
        
        if (request.TranslationMemoriesGuids != null && request.TranslationMemoriesGuids.Any())
        {
            options.ResourceFilter = new PreTransFilter
            {
                TMs = request.TranslationMemoriesGuids.Select(Guid.Parse).ToArray()
            };
        }
        
        options.FragmentAssemblySettings = new FragmentAssemblySettings
        {
            IncludeNum = request.IncludeNumbers ?? true,
            ChangeCase = request.ChangeCase ?? false,
            IncludeAT = request.IncludeAutoTranslations ?? true,
            IncludeFrag = request.IncludeFragments ?? true,
            IncludeNT = request.IncludeNonTranslatables ?? true,
            IncludeTB = request.IncludeTermBases ?? true,
            MinCoverage = request.MinCoverage ?? 50,
            CoverageType = (MatchCoverageType)int.Parse(request.CoverageType ?? "300"),
        };

        var guids = request.DocumentGuids?.Select(Guid.Parse).ToArray();
        if (guids != null && guids.Length != 0)
        {
            var resultInfo = await projectService.Service.PretranslateDocumentsAsync(Guid.Parse(projectRequest.ProjectGuid),
                guids, options);
            
            return new(resultInfo);
        }
        
        var targetLanguages = request.TargetLanguages?.ToArray();
        var result = await projectService.Service.PretranslateProjectAsync(Guid.Parse(projectRequest.ProjectGuid), targetLanguages, options);
        return new(result);
    }

    private List<ServerProjectResourceAssignment> CreateAssignmentsBasedOnResourceType(ResourceType resourceType,
        AddResourceToProjectRequest request)
    {
        var assignments = new List<ServerProjectResourceAssignment>();

        if (request.ObjectIds != null && request.ObjectIds.Any())
        {
            foreach (var objectId in request.ObjectIds)
            {
                assignments.Add(new ServerProjectResourceAssignment()
                {
                    ResourceGuid = Guid.Parse(request.ResourceGuid),
                    ObjectId = objectId,
                    Primary = request.Primary ?? false
                });
            }
        }
        else
        {
            assignments.Add(new ServerProjectResourceAssignment()
            {
                ResourceGuid = Guid.Parse(request.ResourceGuid),
                Primary = request.Primary ?? false
            });
        }

        return assignments;
    }
}