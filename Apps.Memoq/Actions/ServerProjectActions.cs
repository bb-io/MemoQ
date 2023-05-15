using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.Memoq.Models.ServerProjects.Requests;
using Apps.Memoq.Models.ServerProjects.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using MQS.ServerProject;

namespace Apps.Memoq.Actions
{
    [ActionList]
    public class ServerProjectActions
    {
        [Action("List all projects", Description = "List all projects")]
        public ListAllProjectsResponse ListAllProjects(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {
            var projectService = new MemoqServiceFactory<IServerProjectService>(ApplicationConstants.ProjectServiceUrl, authenticationCredentialsProviders);
            return new ListAllProjectsResponse()
            {
                ServerProjects = projectService.Service.ListProjects(new ServerProjectListFilter())
            };
        }

        [Action("Get project", Description = "Get project by UId")]
        public ServerProjectInfo GetProject(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] string projectGuid)
        {
            var projectService = new MemoqServiceFactory<IServerProjectService>(ApplicationConstants.ProjectServiceUrl, authenticationCredentialsProviders);
            return projectService.Service.GetProject(Guid.Parse(projectGuid));
        }

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
                CallbackWebServiceUrl = request.CallbackUrl
            };
            var projectService = new MemoqServiceFactory<IServerProjectService>(ApplicationConstants.ProjectServiceUrl, authenticationCredentialsProvider);
            var guid = projectService.Service.CreateProject2(newProject);
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

        [Action("Delete project", Description = "Delete project")]
        public void DeleteProject(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] string projectGuid)
        {
            var projectService = new MemoqServiceFactory<IServerProjectService>(ApplicationConstants.ProjectServiceUrl, authenticationCredentialsProviders);
            projectService.Service.DeleteProject(Guid.Parse(projectGuid));
        }
    }
}
