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

        [Action("Add target language to project", Description = "Add target language to project by code")]
        public void AddNewTargetLanguageToProject(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
                       [ActionParameter] string projectGuid, [ActionParameter] string targetLangCode)
        {
            var projectService = new MemoqServiceFactory<IServerProjectService>(ApplicationConstants.ProjectServiceUrl, authenticationCredentialsProviders);
            projectService.Service.AddLanguageToProject(Guid.Parse(projectGuid), new ServerProjectAddLanguageInfo() { TargetLangCode = targetLangCode } );
        }

        [Action("Create a project", Description = "Creates a new project in memoQ")]
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

        [Action("Create a project from a template", Description = "Creates a new project based on an existing memoQ project template")]
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
