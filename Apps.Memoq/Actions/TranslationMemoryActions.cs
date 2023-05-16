using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.Memoq.Models.TranslationMemories.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using MQS.FileManager;
using MQS.TM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memoq.Actions
{
    [ActionList]
    public class TranslationMemoryActions
    {
        [Action("Create translation memory", Description = "Create translation memory")]
        public TMInfo CreateTranslationMemory(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] CreateTranslationMemoryRequest input)
        {
            using var tmService = new MemoqServiceFactory<ITMService>(ApplicationConstants.TranslationMemoryServiceUrl, authenticationCredentialsProviders);
            var tmGuid = tmService.Service.CreateAndPublish(new TMInfo() { Name = input.Name });
            return tmService.Service.GetTMInfo(tmGuid);
        }

        [Action("Import TMX file", Description = "Import TMX file")]
        public void ImportTMXFile(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] ImportTMXFileRequest input)
        {
            using var tmService = new MemoqServiceFactory<ITMService>(ApplicationConstants.TranslationMemoryServiceUrl, authenticationCredentialsProviders);
            UploadTMXFile(input.File, tmService.Service, Guid.Parse(input.tmGuid));
        }

        private void UploadTMXFile(byte[] file, ITMService service, Guid tmGuid)
        {
            using var fileStream = new MemoryStream(file);
            const int chunkSize = 500000;
            byte[] chunkBytes = new byte[chunkSize];
            int bytesRead;
            Guid sessionId = service.BeginChunkedTMXImport(tmGuid);
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
                service.AddNextTMXChunk(sessionId, dataToUpload);
            }
            if (sessionId != Guid.Empty)
                service.EndChunkedTMXImport(sessionId);
        }
    }
}
