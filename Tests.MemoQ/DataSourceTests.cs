using Tests.MemoQ.Base;
using Apps.Memoq.DataSourceHandlers;
using Apps.MemoQ.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common.Dynamic;

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
