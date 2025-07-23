using Apps.Memoq.Events.Polling;
using Apps.MemoQ.Events.Polling.Models;
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
        public async Task TestOnTaskStatusChanged()
        {
            var polling = new PollingList(InvocationContext);
            var taskId = "f8109c5c-b8a8-4acb-8120-3089bbb0b45e";
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
    }
}
