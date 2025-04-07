using Apps.Memoq.Actions;
using Apps.Memoq.DataSourceHandlers;
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
        var handler = new ServerProjectActions(InvocationContext);

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
}
