using Apps.Memoq.DataSourceHandlers;
using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.Termbases.Requests;

public class CreateTermbaseRequest
{
    [Display("Existing termbase ID", Description = "Provide an ID to update an existing glossary. Leave empty to create a new one.")]
    [DataSource(typeof(TermbaseDataHandler))]
    public string? ExistingTermbaseId { get; set; }

    [Display("Is QTerm", Description = "Whether the termbase should be marked as QTerm.")]
    public bool? IsQTerm { get; set; }

    [Display("Glossary name", Description = "Name of the glossary to be created or updated.")]
    public string? Name { get; set; }

    [Display("Description", Description = "Description of the termbase.")]
    public string? Description { get; set; }

    [Display("Client", Description = "The client associated with the termbase.")]
    public string? Client { get; set; }

    [Display("Project", Description = "The project associated with the termbase.")]
    public string? Project { get; set; }

    [Display("Domain", Description = "The domain associated with the termbase.")]
    public string? Domain { get; set; }

    [Display("Subject", Description = "The subject associated with the termbase.")]
    public string? Subject { get; set; }

    [Display("Is moderated", Description = "Whether the termbase requires moderation.")]
    public bool? IsModerated { get; set; }

    [Display("Allow add new languages", Description = "Allow adding new languages during the update.")]
    [StaticDataSource(typeof(YesNoDataHandler))]
    public bool? AllowAddNewLanguages { get; set; }

    [Display("Overwrite entries with same ID", Description = "Overwrite entries with the same ID during the update.")]
    [StaticDataSource(typeof(YesNoDataHandler))]
    public bool? OverwriteEntriesWithSameId { get; set; }

    [Display("Late disclosure", Description = "Can be applied only when 'Is moderated' parameter set to 'True'. When set " +
                                              "to 'False', entries appear immediately in a moderated term base. If the " +
                                              "terminologist rejects one later, it will be removed from the term base.")]
    public bool? ModLateDisclosure { get; set; }
}