using Apps.Memoq.Actions;
using Apps.Memoq.Models.ServerProjects.Requests;
using Apps.MemoQ.Models.ServerProjects.Requests;
using Newtonsoft.Json;
using Tests.MemoQ.Base;

namespace Tests.MemoQ;

[TestClass]
public class ServerProjectTests : TestBase
{
    private const string PostTranslationProjectGuid = "ec423da3-ecef-ed11-85f6-d05099f919f4";
    private const string LqaProjectGuid = "275b8794-c7e8-f111-8765-a8a15994f72e";

    [TestMethod]
    public async Task Projects_returns_report_values()
    {
        var handler = new ServerProjectActions(InvocationContext, FileManager);

        var result = await handler.SearchEditReports(new ProjectRequest { ProjectGuid = "10bd767d-3ce2-ef11-875f-a8a15994f72e" });
        foreach (var item in result.Reports)
        {
            Console.WriteLine($"{item.ReportId}: {item.Note}");
        }
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task UpdateProject_returns_values()
    {
        var handler = new ServerProjectActions(InvocationContext, FileManager);

        await handler.UpdateProject(new ProjectRequest
        {
            ProjectGuid = "3c8014cf-c3f0-ed11-85f6-d05099f919f4"
        }, new UpdateProjectRequest
        {
            CallbackUrl = "https://webhook.test.com",
            Description = "Test description 2",
            Subject = "Test subject 3",
            Domain = "Test domain 2",
            Client = "Test client 2",
            Deadline = DateTime.Now.AddDays(7)
        });
    }

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
    public async Task GenerateLqaStatisticsReport_IsSuccess()
    {
        var actions = new ServerProjectActions(InvocationContext, FileManager);
        var project = new ProjectRequest { ProjectGuid = LqaProjectGuid };
        var input = new LqaReportRequest
        {
            DocumentIds = [],
            ExportFormat = "None",
            CsvSeparator = "Semicolon"
        };

        var result = await actions.GenerateLqaStatisticsReport(project, input);

        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.ProjectStatistics);
        Assert.IsTrue(result.DocumentStatistics.Any());
        Assert.IsTrue(result.ModelStatistics.Any());
        Assert.IsTrue(result.DocumentStatistics.All(statistics =>
            !statistics.NormalizedPenaltyPoint.HasValue || double.IsFinite(statistics.NormalizedPenaltyPoint.Value)));
        Assert.IsTrue(result.ModelStatistics.All(statistics =>
            !statistics.NormalizedPenaltyPoint.HasValue || double.IsFinite(statistics.NormalizedPenaltyPoint.Value)));
    }

    [TestMethod]
    public async Task GenerateLqaRejectedSegmentsReport_IsSuccess()
    {
        var actions = new ServerProjectActions(InvocationContext, FileManager);
        var project = new ProjectRequest { ProjectGuid = LqaProjectGuid };
        var input = new LqaReportRequest
        {
            DocumentIds = [],
            ExportFormat = "None",
            CsvSeparator = "Semicolon"
        };

        var result = await actions.GenerateLqaRejectedSegmentsReport(project, input);

        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Documents.Any());
        Assert.IsTrue(result.Documents.SelectMany(document => document.RejectedSegments).Any());
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

    [TestMethod]
    public async Task GeneratePostTranslationAnalysisReport_IsSuccess()
    {
        var actions = new ServerProjectActions(InvocationContext, FileManager);

        var result = await actions.GeneratePostTranslationAnalysisReport(
            new ProjectRequest { ProjectGuid = "3bd942d0-93ed-f011-875f-a8a15994f72e" },
            new PostTranslationAnalysisRequest
            {
                StoreReportInProject = true
            });

        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task SearchPostTranslationAnalysisReports_IsSuccess()
    {
        var actions = new ServerProjectActions(InvocationContext, FileManager);

        var result = await actions.SearchPostTranslationAnalysisReports(
            new ProjectRequest { ProjectGuid = "3bd942d0-93ed-f011-875f-a8a15994f72e" });

        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsNotNull(result);
    }
}
