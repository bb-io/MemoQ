using Apps.Memoq.DataSourceHandlers;
using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.MemoQ.Models.Termbases.Requests
{
    public class UpdateTermbaseRequest
    {
        [Display("File termbase ID", Description = "Provide File ID to update an existing glossary.")]
        //[DataSource(typeof(TermbaseDataHandler))]
        public string FileTermbaseGuid { get; set; }

        [Display("Existing termbase ID", Description = "Provide an ID of an existing glossary.")]
        [DataSource(typeof(TermbaseDataHandler))]
        public string ExistingTermbaseGuid { get; set; }

        [Display("Allow add new languages", Description = "Allow adding new languages during the update.")]
        [StaticDataSource(typeof(YesNoDataHandler))]
        public bool? AllowAddNewLanguages { get; set; }

        [Display("Overwrite entries with same ID", Description = "Overwrite entries with the same ID during the update.")]
        [StaticDataSource(typeof(YesNoDataHandler))]
        public bool? OverwriteEntriesWithSameId { get; set; }
    }
}
