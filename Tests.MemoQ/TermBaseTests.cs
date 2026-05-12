using Newtonsoft.Json;
using Tests.MemoQ.Base;
using Apps.Memoq.Actions;
using Apps.Memoq.Models.Termbases.Requests;

namespace Tests.MemoQ;

[TestClass]
public class TermBaseTests : TestBase
{
    [TestMethod]
    public async Task ExportTermbase_IsSuccess()
    {
		// Arrange
		var action = new TermBaseActions(InvocationContext, FileManager);
        var request = new TermbaseRequest { TermbaseId = "f4ffb6df-6279-4afc-8e01-31dccd0f08f4" };

        // Act
        var result = await action.ExportTermbase(request, false, null, null);

        // Assert
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsNotNull(result);
    }
}
