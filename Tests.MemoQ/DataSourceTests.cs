using Apps.Memoq.Actions;
using Apps.Memoq.DataSourceHandlers;
using Apps.Memoq.Models.ServerProjects.Requests;
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

    [TestMethod]
    public async Task UpdateProject_returns_values()
    {
        var handler = new ServerProjectActions(InvocationContext, FileManager);

        await handler.UpdateProject(new ProjectRequest
        {
            ProjectGuid = "3c8014cf-c3f0-ed11-85f6-d05099f919f4"
        }, new UpdateProjectRequest 
        { 
            CallbackUrl= "https://webhook.test.com", 
            Description= "Test description 2",
            Subject="Test subject 3",
            Domain= "Test domain 2",
            Client= "Test client 2",
            Deadline = DateTime.Now.AddDays(7)
        });
        Assert.IsTrue(true);
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
