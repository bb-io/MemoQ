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

            var response = await action.DownloadFileByGuid( new DownloadFileRequest { ProjectGuid = "b3398340-2bdd-f011-97cc-005056ba375d", 
                DocumentGuid = "31e2e2a5-b3a8-4590-8ecf-03400e2a89e4"
            });

            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(response));
            Assert.IsNotNull(response);
        }
    }
}
