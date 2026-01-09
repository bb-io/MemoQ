using Apps.Memoq.Actions;
using Apps.Memoq.Models.ServerProjects.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.MemoQ.Base;

namespace Tests.MemoQ
{
    [TestClass]
    public class FileTests :TestBase
    {
        [TestMethod]
        public async Task DownloadFileByGuid_IsSuccess()
        {
            var action = new FileActions(InvocationContext, FileManager);

            var response = await action.DownloadFileByGuid( new DownloadFileRequest { ProjectGuid = "10bd767d-3ce2-ef11-875f-a8a15994f72e", 
                DocumentGuid = "0d928a55-8070-48db-9e3e-00723fe85cfd"
            });

            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(response));
            Assert.IsNotNull(response);
        }
    }
}
