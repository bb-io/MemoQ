using Tests.MemoQ.Base;
using Apps.Memoq.Actions;
using Blackbird.Applications.Sdk.Common.Files;
using Apps.Memoq.Models.TranslationMemories.Requests;
using Apps.MemoQ.Models.TranslationMemories.Requests;

namespace Tests.MemoQ;

[TestClass]
public class TranslationMemoryTests : TestBase
{
	private readonly TranslationMemoryActions _actions;

	public TranslationMemoryTests() => _actions = new TranslationMemoryActions(InvocationContext, FileManager);

    [TestMethod]
    public async Task CreateTranslationMemory_ReturnsCreatedTm()
    {
		// Arrange
		var request = new CreateTranslationMemoryRequest 
		{ 
			Name = "test for TMX",
			SourceLanguage = "eng-US",
			TargetLanguage = "tur"
        };

		// Act
		var result = await _actions.CreateTranslationMemory(request);

		// Assert
		PrintJsonResult(result);
		Assert.IsNotNull(result);
	}

    [TestMethod]
    public async Task ImportTmxFile_IsSuccess()
    {
        // Arrange
        var handler = new TranslationMemoryActions(InvocationContext, FileManager);
        var input = new ImportTmxFileRequest
        {
            File = new FileReference { Name = "test.tmx" },
            TmGuid = "6d8c9a49-421d-4991-ae90-fcce12c6f3cd"
        };

        // Act
        var result = await handler.ImportTmxFile(input);

        // Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task ExportTranslationMemory_ReturnsExportedTm()
    {
        // Arrange
        var input = new ExportTranslationMemoryRequest
        {
            TmGuid = "6d8c9a49-421d-4991-ae90-fcce12c6f3cd"
        };

        // Act
        var result = await _actions.ExportTranslationMemory(input);

        // Assert
        Console.WriteLine(result.File.Name);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task ListTranslationMemories_ReturnsTms()
    {
        var handler = new TranslationMemoryActions(InvocationContext, FileManager);

        var result = await handler.ListTranslationMemories(new Apps.Memoq.Models.LanguagesRequest
        {
            //Client = "Test client" ,
            //Project = "XLIFF project",
            //Domain = "deepL",
            //Subject = "Xliff sample",
            //NameOrDescription = "blackbird",
            //LastModifiedAfter = new DateTime(2024, 7, 1),
            //LastModifiedBefore = new DateTime(2024, 7, 31, 23, 59, 59),
            SourceLanguage = "eng",
            TargetLanguage = "dut",
        });

        PrintJsonResult(result);
        Assert.IsNotNull(result);
    }
}
