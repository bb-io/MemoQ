using System.Reflection;
using Apps.Memoq.Actions;
using Apps.Memoq.Models.Termbases;
using Apps.Memoq.Models.Termbases.Requests;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Glossaries.Utils.Converters;
using Blackbird.Applications.Sdk.Glossaries.Utils.Dtos;
using MQS.TB;
using Tests.MemoQ.Base;

namespace Tests.MemoQ;

[TestClass]
public class TermBaseImportTests
{
    private const string GlossaryFileName = "glossary.tbx";
    private const string ExistingTermbaseId = "8d3c1f4e-2b7a-4c55-9e61-0f2a7b9c4d11";

    private string _testFolder = null!;

    [TestInitialize]
    public async Task CreateGlossaryFile()
    {
        _testFolder = Path.Combine(Path.GetTempPath(), $"memoq-termbase-import-{Guid.NewGuid()}");
        Directory.CreateDirectory(Path.Combine(_testFolder, "Input"));

        var glossary = new Glossary(new List<GlossaryConceptEntry>
        {
            new("entry-1", new List<GlossaryLanguageSection>
            {
                new("en", new List<GlossaryTermSection> { new("apple") }),
                new("de", new List<GlossaryTermSection> { new("Apfel") })
            })
        }) { Title = "Test glossary" };

        await using var tbxStream = glossary.ConvertToTbx();
        await using var file = File.Create(Path.Combine(_testFolder, "Input", GlossaryFileName));
        await tbxStream.CopyToAsync(file);
    }

    [TestCleanup]
    public void DeleteGlossaryFile() => Directory.Delete(_testFolder, true);

    [TestMethod]
    public async Task ImportTermbase_ReplaceExistingEntries_PurgesExistingTermbaseBeforeImport()
    {
        var termbaseService = RecordingTermbaseService.Create();

        await ImportGlossary(termbaseService, new CreateTermbaseRequest
        {
            ExistingTermbaseId = ExistingTermbaseId,
            ReplaceExistingEntries = true
        });

        var calledMethods = termbaseService.CalledMethodNames();
        CollectionAssert.Contains(calledMethods, nameof(ITBService.PurgeTBAsync));
        Assert.IsLessThan(calledMethods.IndexOf(nameof(ITBService.BeginChunkedCSVImportAsync)),
            calledMethods.IndexOf(nameof(ITBService.PurgeTBAsync)));
        Assert.AreEqual(Guid.Parse(ExistingTermbaseId),
            termbaseService.Arguments(nameof(ITBService.PurgeTBAsync))[0]);
    }

    [TestMethod]
    public async Task ImportTermbase_WithoutReplaceExistingEntries_KeepsExistingEntries()
    {
        var termbaseService = RecordingTermbaseService.Create();

        await ImportGlossary(termbaseService, new CreateTermbaseRequest
        {
            ExistingTermbaseId = ExistingTermbaseId
        });

        var calledMethods = termbaseService.CalledMethodNames();
        CollectionAssert.DoesNotContain(calledMethods, nameof(ITBService.PurgeTBAsync));
        CollectionAssert.Contains(calledMethods, nameof(ITBService.AddNextCSVChunkAsync));
    }

    [TestMethod]
    public async Task ImportTermbase_ReplaceExistingEntriesForNewTermbase_DoesNotPurge()
    {
        var termbaseService = RecordingTermbaseService.Create();

        await ImportGlossary(termbaseService, new CreateTermbaseRequest
        {
            ReplaceExistingEntries = true
        });

        CollectionAssert.DoesNotContain(termbaseService.CalledMethodNames(), nameof(ITBService.PurgeTBAsync));
    }

    [TestMethod]
    public async Task ImportTermbase_OverwriteEntriesWithSameId_UpdatesEntriesBasedOnIds()
    {
        var termbaseService = RecordingTermbaseService.Create();

        await ImportGlossary(termbaseService, new CreateTermbaseRequest
        {
            ExistingTermbaseId = ExistingTermbaseId,
            OverwriteEntriesWithSameId = true
        });

        var importSettings = (CSVImportSettings)termbaseService.Arguments(nameof(ITBService.BeginChunkedCSVImportAsync))[1]!;
        Assert.IsTrue(importSettings.UpdateBasedOnEntryIDs);
    }

    private async Task ImportGlossary(RecordingTermbaseService termbaseService, CreateTermbaseRequest request)
    {
        var actions = new TermBaseActionsWithTermbaseService(new InvocationContext(), new FileManager(_testFolder),
            termbaseService.AsTermbaseService());

        await actions.ImportTermbase(
            new GlossaryWrapper { Glossary = new FileReference { Name = GlossaryFileName } }, request);
    }

    private class TermBaseActionsWithTermbaseService(
        InvocationContext invocationContext,
        IFileManagementClient fileManagementClient,
        ITBService termbaseService) : TermBaseActions(invocationContext, fileManagementClient)
    {
        protected override ITBService TermbaseService => termbaseService;
    }
}

public class RecordingTermbaseService : DispatchProxy
{
    private readonly List<(string MethodName, object?[] Arguments)> _calls = new();

    public static RecordingTermbaseService Create() =>
        (RecordingTermbaseService)(object)DispatchProxy.Create<ITBService, RecordingTermbaseService>();

    public ITBService AsTermbaseService() => (ITBService)(object)this;

    public List<string> CalledMethodNames() => _calls.Select(call => call.MethodName).ToList();

    public object?[] Arguments(string methodName) => _calls.First(call => call.MethodName == methodName).Arguments;

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        _calls.Add((targetMethod!.Name, args ?? Array.Empty<object?>()));
        return CompletedTaskFor(targetMethod.ReturnType);
    }

    private static object? CompletedTaskFor(Type returnType)
    {
        if (returnType == typeof(Task))
            return Task.CompletedTask;

        if (!returnType.IsGenericType || returnType.GetGenericTypeDefinition() != typeof(Task<>))
            return null;

        var resultType = returnType.GetGenericArguments()[0];
        return typeof(Task).GetMethod(nameof(Task.FromResult))!
            .MakeGenericMethod(resultType)
            .Invoke(null, new[] { EmptyResultFor(resultType) });
    }

    private static object? EmptyResultFor(Type resultType)
    {
        if (resultType.IsArray)
            return Array.CreateInstance(resultType.GetElementType()!, 0);

        return resultType.IsValueType ? Activator.CreateInstance(resultType) : null;
    }
}
