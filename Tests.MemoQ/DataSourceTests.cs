using Apps.Memoq.Actions;
using Apps.Memoq.DataSourceHandlers;
using Apps.MemoQ.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Tests.MemoQ.Base;

namespace Tests.MemoQ;
[TestClass]
public class DataSourceTests : TestBase
{
    [TestMethod]
    public async Task Projects_returns_values()
    {
        var handler = new ProjectDataHandler(InvocationContext);

        var result = await handler.GetDataAsync(new DataSourceContext { }, CancellationToken.None);
        foreach (var item in result)
        {
            Console.WriteLine($"{item.Value}: {item.DisplayName}");
        }
        Assert.IsTrue(result.Count() > 0);
    }

    [TestMethod]
    public async Task Projects_returns_report_values()
    {
        var handler = new ServerProjectActions(InvocationContext, FileManager);

        var result = await handler.SearchEditReports(new Apps.Memoq.Models.ServerProjects.Requests.ProjectRequest { ProjectGuid = "10bd767d-3ce2-ef11-875f-a8a15994f72e" });
        foreach (var item in result.Reports)
        {
            Console.WriteLine($"{item.ReportId}: {item.Note}");
        }
        Assert.IsNotNull(result);   
    }



    [TestMethod]
    public async Task Report_returns_report_values()
    {
        var handler = new FileActions(InvocationContext, FileManager);

        var result = await handler.GetEditDistanceReport(
            new Apps.Memoq.Models.ServerProjects.Requests.ProjectRequest { ProjectGuid = "10bd767d-3ce2-ef11-875f-a8a15994f72e" },
            "a25715b6-c004-f011-875f-a8a15994f72e");
       
        Assert.IsNotNull(result);
    }


    //ListTranslationMemories

    [TestMethod]
    public async Task Translation_returns_memory_values()
    {
        var handler = new TranslationMemoryActions(InvocationContext, FileManager);

        var result = await handler.ListTranslationMemories(new Apps.Memoq.Models.LanguagesRequest { });

        var json = System.Text.Json.JsonSerializer.Serialize(result, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine(json);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task ProjectTemplateDataHandler_returns_values()
    {
        var handler = new ProjectTemplateDataHandler(InvocationContext);

        var result = await handler.GetDataAsync(new DataSourceContext { }, CancellationToken.None);

        foreach (var item in result) 
        {
            Console.WriteLine($"Name {item.DisplayName} , ID: {item.Value}");
        }

        Assert.IsNotNull(result);
    }
}
