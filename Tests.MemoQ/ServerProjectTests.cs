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

        [TestMethod]
        public async Task GenerateFuzzyEditDistanceReport_IsSuccess()
        {
            var handler = new ServerProjectActions(InvocationContext);

            var result = await handler.GenerateFuzzyEditDistanceReport(new ProjectRequest { ProjectGuid = "ec423da3-ecef-ed11-85f6-d05099f919f4"},
                new EditDistanceStatisticsRequest { StoreReportInProject = true, LanguageCodes = ["dut-NL"] });
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(json);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task GenerateLevenshteinEditDistanceReport_IsSuccess()
        {
            var handler = new ServerProjectActions(InvocationContext);

            var result = await handler.GenerateLevenshteinEditDistanceReport(new ProjectRequest { ProjectGuid = "ec423da3-ecef-ed11-85f6-d05099f919f4"},
                new EditDistanceStatisticsRequest { StoreReportInProject = true, LanguageCodes = ["dut-NL"] });
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(json);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task CreateProjectFromTemplate_IsSuccess()
        {
            var handler = new ServerProjectActions(InvocationContext);

            var result = await handler.CreateProjectFromTemplate(new CreateProjectTemplateRequest 
            { 
                ProjectName = "Test from API5463",
                SourceLangCode = "eng-GB",
                TargetLangCodes = ["fre-CA"],
                ProjectMetadata = "1212284379468771",
                Client = "API Client",
                TemplateGuid = "16000000-0000-0000-0000-00001b46f17c"

            });
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(json);

            Assert.IsTrue(true);
        }
    }
}
