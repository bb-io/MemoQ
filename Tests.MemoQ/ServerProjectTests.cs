using Apps.Memoq.Actions;
using Apps.Memoq.Models.ServerProjects.Requests;
using Apps.MemoQ.Models.ServerProjects.Requests;
using Newtonsoft.Json;
using Tests.MemoQ.Base;

namespace Tests.MemoQ;

[TestClass]
public class ServerProjectTests : TestBase
{
    [TestMethod]
    public async Task SetCustomField_IsSuccess()
    {
        var actions = new ServerProjectActions(InvocationContext, FileManager);

        var value = "Hello";

        await actions.SetCustomField(new ProjectRequest { ProjectGuid = "10bd767d-3ce2-ef11-875f-a8a15994f72e" },
            new GetCustomFieldRequest { Field = "" }, value);
    }

    [TestMethod]
    public async Task GenerateFuzzyEditDistanceReport_IsSuccess()
    {
        var actions = new ServerProjectActions(InvocationContext, FileManager);

        var result = await actions.GenerateFuzzyEditDistanceReport(new ProjectRequest { ProjectGuid = "ec423da3-ecef-ed11-85f6-d05099f919f4"},
            new EditDistanceStatisticsRequest { StoreReportInProject = true, LanguageCodes = ["dut-NL"] });

        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GenerateLevenshteinEditDistanceReport_IsSuccess()
    {
        var actions = new ServerProjectActions(InvocationContext, FileManager);

        var result = await actions.GenerateLevenshteinEditDistanceReport(new ProjectRequest { ProjectGuid = "ec423da3-ecef-ed11-85f6-d05099f919f4"},
            new EditDistanceStatisticsRequest { StoreReportInProject = true, LanguageCodes = ["dut-NL"] });

        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task CreateProjectFromTemplate_IsSuccess()
    {
        var actions = new ServerProjectActions(InvocationContext, FileManager);

        var result = await actions.CreateProjectFromTemplate(new CreateProjectTemplateRequest 
        { 
            ProjectName = "Test from API5463ро",
            SourceLangCode = "eng-GB",
            TargetLangCodes = ["fre-CA"],
            ProjectMetadata = "1212751453167361",
            Client = "API Client",
            TemplateGuid = "5d57a482-3a35-4038-9013-3ec5d5d298f4"

        });

        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task RunQaChecks_IsSuccess()
    {
        // Arrange
        var actions = new ServerProjectActions(InvocationContext, FileManager);
        var project = new ProjectRequest { ProjectGuid = "cc189b80-24dd-f011-875f-a8a15994f72e" };
        var qaRequest = new RunQaChecksRequest 
        { 
            ReportType = "None", 
            ReportLanguage = "de"
        };

        // Act
        var result = await actions.RunQaChecks(project, qaRequest);

        // Assert
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsNotNull(result);
    }
}
