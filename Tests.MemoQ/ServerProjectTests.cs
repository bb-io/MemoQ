using Apps.Memoq.Actions;
using Apps.Memoq.Models.ServerProjects.Requests;
using Apps.MemoQ.Models.ServerProjects.Requests;
using Tests.MemoQ.Base;

namespace Tests.MemoQ
{
    [TestClass]
    public class ServerProjectTests : TestBase
    {

        [TestMethod]
        public async Task Set_custom_values()
        {
            var handler = new ServerProjectActions(InvocationContext);

            var value = "Hello";

            await handler.SetCustomField(new ProjectRequest { ProjectGuid = "10bd767d-3ce2-ef11-875f-a8a15994f72e" },
                new GetCustomFieldRequest { Field = "" }, value);

            Assert.IsTrue(true);
        }
    }
}
