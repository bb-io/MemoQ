using Apps.Memoq.Events.Polling;
using Apps.Memoq.Events.Polling.Models.Memory;
using Apps.Memoq.Models.ServerProjects.Requests;
using Apps.MemoQ.Events.Polling.Models;
using Apps.MemoQ.Events.Polling.Models.Memory;
using Blackbird.Applications.Sdk.Common.Polling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.MemoQ.Base;

namespace Tests.MemoQ
{
    [TestClass]
    public  class PollingTests :TestBase
    { 
        [TestMethod]
        public async Task OnTaskStatusChanged_IsSuccess()
        {
            var polling = new PollingList(InvocationContext);
            var taskId = "5b070390-c8be-4284-a86b-061080a6a2ad";
            var targetTaskStatus = "Completed"; 
            var request = new PollingEventRequest<TaskStatusMemory>
            {
                Memory = new TaskStatusMemory
                {
                    //Status = "Pending",
                    //LastCheckDate = DateTime.UtcNow.AddMinutes(-5)
                }
            };
            var response = await polling.OnTaskStatusChanged(request, taskId, targetTaskStatus);

            var json = new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            };
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(response, json));
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task OnProjectStatusChanged_IsSuccess()
        {
            var polling = new PollingList(InvocationContext);
            var targetProjectStatus = "WrappedUp";
            var project= new ProjectRequest { ProjectGuid = "49264708-666c-f011-875f-a8a15994f72e" };
            var request = new PollingEventRequest<EntityChangedMemory>
            {
                Memory = new EntityChangedMemory
                {
                    Status = "Live",
                    LastModificationDate = DateTime.UtcNow.AddMinutes(-30)
                }

            };
            var response = await polling.OnProjectStatusChanged(request, project, targetProjectStatus);

            var json = new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            };
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(response, json));
            Assert.IsNotNull(response);
        }


        [TestMethod]
        public async Task OnAllFilesDelivered_IsSuccess()
        {
            var polling = new PollingList(InvocationContext);
            var project = new ProjectRequest { ProjectGuid = "3bd942d0-93ed-f011-875f-a8a15994f72e" };
            var request = new PollingEventRequest<AllFilesDeliveredMemory>
            {
            };
            var status = new TranslationFileStatus { Status = "TranslationFinished" };
            var response = await polling.OnAllFilesDelivered(request, project, status);

            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(response));
            Assert.IsNotNull(response);
        }
    }
}
