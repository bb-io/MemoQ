using Apps.Memoq.Constants;
using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.Memoq.Models.Dto;
using Apps.Memoq.Models.TranslationMemories.Requests;
using Apps.Memoq.Models.TranslationMemories.Responses;
using Apps.Memoq.Utils.FileUploader;
using Apps.Memoq.Utils.FileUploader.Managers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Parsers;
using MQS.TM;

namespace Apps.Memoq.Actions
{
    [ActionList]
    public class TranslationMemoryActions : BaseInvocable
    {
        private IEnumerable<AuthenticationCredentialsProvider> Creds =>
            InvocationContext.AuthenticationCredentialsProviders;

        public TranslationMemoryActions(InvocationContext invocationContext) : base(invocationContext)
        {
        }

        [Action("List translation memories", Description = "List translation memories")]
        public ListTranslationMemoriesResponse ListTranslationMemories(
            [ActionParameter] LanguagesRequest input)
        {
            using var tmService = new MemoqServiceFactory<ITMService>(
                ApplicationConstants.TranslationMemoryServiceUrl, Creds);

            var response = tmService.Service.ListTMs(input.SourceLanguage, input.TargetLanguage);
            var translationMemories = response.Select(x => new TmDto(x)).ToArray();
            
            return new()
            {
                TranslationMemories = translationMemories
            };
        }

        [Action("Create translation memory", Description = "Create translation memory")]
        public TmDto CreateTranslationMemory([ActionParameter] CreateTranslationMemoryRequest input)
        {
            using var tmService = new MemoqServiceFactory<ITMService>(
                ApplicationConstants.TranslationMemoryServiceUrl, Creds);

            var tmGuid = tmService.Service.CreateAndPublish(new()
            {
                Name = input.Name,
                SourceLanguageCode = input.SourceLanguage,
                TargetLanguageCode = input.TargetLanguage,
                Client = input.Client,
                Domain = input.Domain,
                Subject = input.Subject,
                AllowMultiple = input.AllowMultiple ?? default,
                AllowReverseLookup = input.AllowReverseLookup ?? default,
                CreatorUsername = input.CreatorUsername,
                OptimizationPreference = EnumParser.Parse<TMOptimizationPreference>(input.OptimizationPreference,
                    nameof(input.OptimizationPreference), EnumValues.TMOptimizationPreference) ?? default,
                StoreDocumentFullPath = input.StoreDocumentFullPath ?? default,
                StoreDocumentName = input.StoreDocumentName ?? default,
                StoreFormatting = input.StoreFormatting ?? default,
                TMEngineType =
                    EnumParser.Parse<TMEngineType>(input.TMEngineType, nameof(input.TMEngineType),
                        EnumValues.TMEngineType) ?? default,
                UseContext = input.UseContext ?? default,
                UseIceSpiceContext = input.UseIceSpiceContext ?? default,
            });

            var response = tmService.Service.GetTMInfo(tmGuid);
            return new(response);
        }

        [Action("Import TMX file", Description = "Import TMX file")]
        public ImportTmxFileResponse ImportTMXFile([ActionParameter] ImportTMXFileRequest input)
        {
            using var tmService = new MemoqServiceFactory<ITMService>(
                ApplicationConstants.TranslationMemoryServiceUrl, Creds);

            var manager = new TMXUploadManager(tmService.Service);
            var result = FileUploader.UploadFile(input.File, manager, input.TmGuid);

            return new()
            {
                Guid = result.ToString()
            };
        }
    }
}