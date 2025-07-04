using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.Memoq.Utils.FileUploader.Managers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using DocumentFormat.OpenXml.Drawing;
using MQS.FileManager;
using MQS.Security;
using MQS.ServerProject;
using MQS.TasksService;
using MQS.TB;
using MQS.TM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.MemoQ;
public class MemoqInvocable : BaseInvocable
{
    protected AuthenticationCredentialsProvider[] Creds =>
        InvocationContext.AuthenticationCredentialsProviders.ToArray();

    public MemoqInvocable(InvocationContext invocationContext) : base(invocationContext)
    {
        
    }
    protected MemoqServiceFactory<IServerProjectService> ProjectService => new MemoqServiceFactory<IServerProjectService>(SoapConstants.ProjectServiceUrl, Creds);
    protected MemoqServiceFactory<IFileManagerService> FileService => new MemoqServiceFactory<IFileManagerService>(SoapConstants.FileServiceUrl, Creds);
    protected MemoqServiceFactory<ITMService> TmService => new MemoqServiceFactory<ITMService>(SoapConstants.TranslationMemoryServiceUrl, Creds);
    protected MemoqServiceFactory<ITasksService> TaskService => new MemoqServiceFactory<ITasksService>(SoapConstants.TaskServiceUrl, Creds);
    protected MemoqServiceFactory<ITBService> TbService => new MemoqServiceFactory<ITBService>(SoapConstants.TermBasesServiceUrl, Creds);
    protected MemoqServiceFactory<ISecurityService> SecurityService => new MemoqServiceFactory<ISecurityService>(SoapConstants.SecurityServiceUrl, Creds);
    protected MemoqServiceFactory<IResourceService> ResourceService => new MemoqServiceFactory<IResourceService>(SoapConstants.ResourceServiceUrl, Creds);

    protected FileUploadManager FileUploadManager => new FileUploadManager(FileService.Service);
    protected TmxUploadManager TmxUploadManager => new TmxUploadManager(TmService.Service);


    private const int MaxRetries = 3;
    private static readonly TimeSpan RetryDelay = TimeSpan.FromSeconds(10);
    protected async Task ExecuteWithHandling(Func<Task> func)
    {
        int attempt = 0;
        while (true)
        {   
            try
            {
                await func();
                return;
            }
            catch (Exception ex) when (ex.Message.Contains("cannot start because there is another operation", StringComparison.OrdinalIgnoreCase))
            {
                attempt++;

                if (attempt >= MaxRetries)
                    throw new PluginApplicationException("MemoQ is still busy after multiple attempts. " + ex.Message);

                await Task.Delay(RetryDelay);
            }

            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }
    }

    protected async Task<T> ExecuteWithHandling<T>(Func<Task<T>> func)
    {
        int attempt = 0;
        while (true)
        {
            try
            {
                return await func();
            }
            catch (Exception ex) when (ex.Message.Contains("cannot start because there is another operation", StringComparison.OrdinalIgnoreCase))
            {
                attempt++;

                if (attempt >= MaxRetries)
                    throw new PluginApplicationException("MemoQ is still busy after multiple attempts. " + ex.Message);

                await Task.Delay(RetryDelay);
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }
    }

    private Exception HandleException(Exception ex)
    {
        if (ex.Message == "Message.ResourceNotFound.ProjectTemplate")
            throw new PluginMisconfigurationException("The selected project template does not exist.");
        else if (ex.Message == "An online project with the same name already exists.")
            throw new PluginMisconfigurationException("An online project with the same name already exists. Please configure a unique name.");
        else if (ex.Message == "The name contains invalid characters, or the name is reserved by Windows.")
            return new PluginMisconfigurationException("The name contains invalid characters, or the name is reserved by Windows. Please check the characters you are using in the name.");

        return new PluginApplicationException(ex.Message);
    }

}
