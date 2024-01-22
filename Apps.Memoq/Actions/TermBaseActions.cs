using System.Net.Mime;
using System.Text;
using System.Xml;
using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.Memoq.Models.Termbases;
using Apps.Memoq.Models.Termbases.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Glossaries.Utils.Converters;
using Blackbird.Applications.Sdk.Glossaries.Utils.Dtos;
using MQS.TB;

namespace Apps.Memoq.Actions;

[ActionList]
public class TermBaseActions : BaseInvocable
{
    private readonly IFileManagementClient _fileManagementClient;
    
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public TermBaseActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) 
        : base(invocationContext)
    {
        _fileManagementClient = fileManagementClient;
    }

    [Action("Export glossary", Description = "Export a termbase")]
    public async Task<GlossaryWrapper> ExportTermbase([ActionParameter] TermbaseRequest termbaseRequest,
        [ActionParameter] [Display("Include forbidden terms")] bool? includeForbiddenTerms,
        [ActionParameter] [Display("Title")] string? title,
        [ActionParameter] [Display("Description")] string? description)
    {
        using var tbService = new MemoqServiceFactory<ITBService>(SoapConstants.TermBasesServiceUrl, Creds);
        var termbaseGuid = new Guid(termbaseRequest.TermbaseId);
        var termbase = await tbService.Service.GetTBInfoAsync(termbaseGuid);

        var xmlFileBytes = new List<byte>();
        var sessionId = (await tbService.Service.BeginChunkedMultiTermExportAsync(new() { tbGuid = termbaseGuid }))
            .BeginChunkedMultiTermExportResult;
        
        try
        {
            var chunk = await tbService.Service.GetNextExportChunkAsync(sessionId);

            while (chunk != null && chunk.Length != 0)
            {
                xmlFileBytes.AddRange(chunk);
                chunk = await tbService.Service.GetNextExportChunkAsync(sessionId);
            }

            var glossary = ConvertXmlTermbaseToGlossary(xmlFileBytes.ToArray(), includeForbiddenTerms ?? false,
                title ?? termbase.Name, description ?? termbase.Description);
            var glossaryStream = glossary.ConvertToTBX();

            var glossaryFileReference =
                await _fileManagementClient.UploadAsync(glossaryStream, MediaTypeNames.Text.Xml,
                    $"{termbase.Name}.tbx");
            
            return new() { Glossary = glossaryFileReference };
        }
        finally
        {
            await tbService.Service.EndChunkedExportAsync(sessionId);
        }
    }

    private Glossary ConvertXmlTermbaseToGlossary(byte[] xmlBytes, bool includeForbiddenTerms, string termbaseTitle, 
        string? termbaseDescription)
    {
        var xmlContent = Encoding.Unicode.GetString(xmlBytes);
        
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xmlContent);

        var conceptEntries = new List<GlossaryConceptEntry>();
        var glossary = new Glossary(conceptEntries);
        glossary.Title = termbaseTitle;
        glossary.SourceDescription = termbaseDescription;

        var conceptGroupNodes = xmlDocument.SelectNodes("//mtf/conceptGrp")!;

        foreach (XmlElement conceptNode in conceptGroupNodes)
        {
            var languageGroupNodes = conceptNode.SelectNodes("languageGrp")!;
            var languageSections = new List<GlossaryLanguageSection>();

            foreach (XmlElement languageNode in languageGroupNodes)
            {
                if (!includeForbiddenTerms)
                {
                    var termStatus = languageNode
                        .SelectNodes("termGrp/descripGrp/descrip")?
                        .Cast<XmlElement>()
                        .FirstOrDefault(descriptionNode => descriptionNode.Attributes["type"]?.Value == "Status")?
                        .InnerText;
                    
                    if (termStatus == "Forbidden")
                        continue;
                }
                
                var language = languageNode!.SelectSingleNode("language")!.Attributes!["lang"]!.Value.ToLower();
                var term = languageNode.SelectSingleNode("termGrp/term")!.InnerText;
                var termSection = new GlossaryTermSection(term);
                languageSections.Add(new(language, new List<GlossaryTermSection> { termSection }));
            }

            if (languageSections.Any())
            {
                var conceptDescriptionNodes = conceptNode
                    .SelectNodes("descripGrp/descrip")!
                    .Cast<XmlElement>()
                    .ToArray();
                var id = conceptDescriptionNodes
                    .First(descriptionNode => descriptionNode.Attributes["type"]?.Value == "ID").InnerText;
                var subject = conceptDescriptionNodes
                    .FirstOrDefault(descriptionNode => descriptionNode.Attributes["type"]?.Value == "Subject")
                    ?.InnerText;
                var note = conceptDescriptionNodes
                    .FirstOrDefault(descriptionNode => descriptionNode.Attributes["type"]?.Value == "Note")?.InnerText;
                
                var definition = languageGroupNodes.Cast<XmlElement>()
                    .FirstOrDefault(node =>
                    {
                        var descriptionWithDefinition = node
                            .SelectNodes("descripGrp/descrip")?
                            .Cast<XmlElement>()
                            .FirstOrDefault(descriptionNode => descriptionNode.Attributes["type"]?.Value == "Definition");

                        if (descriptionWithDefinition == null)
                            return false;

                        return true;
                    })?
                    .SelectSingleNode("descripGrp/descrip")!.InnerText;
                
                var entry = new GlossaryConceptEntry(id, languageSections)
                {
                    SubjectField = subject,
                    Definition = definition,
                    Notes = new List<string> { note }
                };
            
                conceptEntries.Add(entry);
            }
        }

        return glossary;
    }
}