using Apps.Memoq.Actions;
using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.Memoq.Models.Termbases;
using Apps.Memoq.Models.Termbases.Requests;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Glossaries.Utils.Converters;
using Blackbird.Applications.Sdk.Glossaries.Utils.Dtos;
using Microsoft.Extensions.Configuration;
using MQS.TB;
using Tests.MemoQ.Base;

namespace Tests.MemoQ;

[TestClass]
public class TermBaseImportOnlineTests : TestBase
{
    private readonly string _testFolder =
        new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()["TestFolder"]!;

    private readonly List<Guid> _createdTermbases = new();

    [TestCleanup]
    public async Task DeleteCreatedTermbases()
    {
        using var termbaseService = new MemoqServiceFactory<ITBService>(SoapConstants.TermBasesServiceUrl, Creds);

        foreach (var termbaseGuid in _createdTermbases)
            await termbaseService.Service.DeleteTBAsync(termbaseGuid);
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task ImportTermbase_ExportedGlossaryWithReplaceExistingEntries_TargetContainsOnlySourceEntries(bool isQTerm)
    {
        var actions = new TermBaseActions(InvocationContext, FileManager);
        var sourceTermbaseId = await CreateTermbase(actions, isQTerm, "source",
            ("apple", "Apfel"), ("pear", "Birne"));
        var targetTermbaseId = await CreateTermbase(actions, isQTerm, "target",
            ("outdated", "veraltet"));
        var sourceGlossary = await ExportTermbaseAsInput(actions, sourceTermbaseId);

        for (var run = 0; run < 2; run++)
            await actions.ImportTermbase(sourceGlossary, new CreateTermbaseRequest
            {
                ExistingTermbaseId = targetTermbaseId,
                IsQTerm = isQTerm,
                ReplaceExistingEntries = true
            });

        var targetTerms = await ExportTermPairs(actions, targetTermbaseId);
        CollectionAssert.AreEquivalent(new[] { "apple|Apfel", "pear|Birne" }, targetTerms);
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task ImportTermbase_ExportedGlossaryWithoutReplaceExistingEntries_AppendsEntries(bool isQTerm)
    {
        var actions = new TermBaseActions(InvocationContext, FileManager);
        var sourceTermbaseId = await CreateTermbase(actions, isQTerm, "source",
            ("apple", "Apfel"), ("pear", "Birne"));
        var targetTermbaseId = await CreateTermbase(actions, isQTerm, "target",
            ("outdated", "veraltet"));
        var sourceGlossary = await ExportTermbaseAsInput(actions, sourceTermbaseId);

        await actions.ImportTermbase(sourceGlossary, new CreateTermbaseRequest
        {
            ExistingTermbaseId = targetTermbaseId,
            IsQTerm = isQTerm
        });

        var targetTerms = await ExportTermPairs(actions, targetTermbaseId);
        CollectionAssert.AreEquivalent(new[] { "outdated|veraltet", "apple|Apfel", "pear|Birne" }, targetTerms);
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task ImportTermbase_OwnExportWithOverwriteEntriesWithSameId_DoesNotDuplicateEntries(bool isQTerm)
    {
        var actions = new TermBaseActions(InvocationContext, FileManager);
        var termbaseId = await CreateTermbase(actions, isQTerm, "overwrite", ("apple", "Apfel"), ("pear", "Birne"));
        var exportedGlossary = await ExportTermbaseAsInput(actions, termbaseId);

        await actions.ImportTermbase(exportedGlossary, new CreateTermbaseRequest
        {
            ExistingTermbaseId = termbaseId,
            IsQTerm = isQTerm,
            OverwriteEntriesWithSameId = true
        });

        var terms = await ExportTermPairs(actions, termbaseId);
        CollectionAssert.AreEquivalent(new[] { "apple|Apfel", "pear|Birne" }, terms);
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task ImportTermbase_GlossaryWithNonNumericIdsAndOverwriteEntriesWithSameId_AddsEntries(bool isQTerm)
    {
        var actions = new TermBaseActions(InvocationContext, FileManager);
        var termbaseId = await CreateTermbase(actions, isQTerm, "foreign-ids", ("apple", "Apfel"));
        var foreignGlossary = WriteGlossaryToInput($"Blackbird import test foreign {Guid.NewGuid():N}",
            new[] { ("cherry", "Kirsche") });

        await actions.ImportTermbase(foreignGlossary, new CreateTermbaseRequest
        {
            ExistingTermbaseId = termbaseId,
            IsQTerm = isQTerm,
            OverwriteEntriesWithSameId = true
        });

        var terms = await ExportTermPairs(actions, termbaseId);
        CollectionAssert.AreEquivalent(new[] { "apple|Apfel", "cherry|Kirsche" }, terms);
    }

    private async Task<string> CreateTermbase(TermBaseActions actions, bool isQTerm, string role,
        params (string English, string German)[] termPairs)
    {
        var termbaseName = $"Blackbird import test {role} {Guid.NewGuid():N}";
        var glossaryFile = WriteGlossaryToInput(termbaseName, termPairs);

        var response = await actions.ImportTermbase(glossaryFile, new CreateTermbaseRequest
        {
            Name = termbaseName,
            IsQTerm = isQTerm
        });

        _createdTermbases.Add(Guid.Parse(response.TermbaseGuid));
        return response.TermbaseGuid;
    }

    private GlossaryWrapper WriteGlossaryToInput(string glossaryName, (string English, string German)[] termPairs)
    {
        var entries = termPairs
            .Select(pair => new GlossaryConceptEntry(Guid.NewGuid().ToString(), new List<GlossaryLanguageSection>
            {
                new("en", new List<GlossaryTermSection> { new(pair.English) }),
                new("de", new List<GlossaryTermSection> { new(pair.German) })
            }))
            .ToList();
        var glossary = new Glossary(entries) { Title = glossaryName };

        var fileName = $"{glossaryName}.tbx";
        Directory.CreateDirectory(Path.Combine(_testFolder, "Input"));
        using (var tbxStream = glossary.ConvertToTbx())
        using (var file = File.Create(Path.Combine(_testFolder, "Input", fileName)))
            tbxStream.CopyTo(file);

        return new GlossaryWrapper { Glossary = new FileReference { Name = fileName } };
    }

    private async Task<GlossaryWrapper> ExportTermbaseAsInput(TermBaseActions actions, string termbaseId)
    {
        var exported = await actions.ExportTermbase(new TermbaseRequest { TermbaseId = termbaseId }, false, null, null);
        var exportedFileName = exported.Glossary.Name;

        File.Copy(Path.Combine(_testFolder, "Output", exportedFileName),
            Path.Combine(_testFolder, "Input", exportedFileName), true);

        return new GlossaryWrapper { Glossary = new FileReference { Name = exportedFileName } };
    }

    private async Task<List<string>> ExportTermPairs(TermBaseActions actions, string termbaseId)
    {
        var exported = await actions.ExportTermbase(new TermbaseRequest { TermbaseId = termbaseId }, false, null, null);
        var exportedPath = Path.Combine(_testFolder, "Output", exported.Glossary.Name);
        Console.WriteLine(await File.ReadAllTextAsync(exportedPath));

        await using var exportedStream = File.OpenRead(exportedPath);
        var glossary = await exportedStream.ConvertFromTbx();

        return glossary.ConceptEntries
            .Select(entry => string.Join('|', new[] { "en", "de" }
                .Select(language => entry.LanguageSections.First(section => section.LanguageCode == language)
                    .Terms.Single().Term)))
            .ToList();
    }
}
