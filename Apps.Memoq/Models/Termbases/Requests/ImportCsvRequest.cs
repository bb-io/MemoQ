using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Memoq.DataSourceHandlers;
using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.MemoQ.Models.Termbases.Requests
{
    public class ImportCsvRequest
    {
        [Display("Existing termbase ID", Description = "Provide an ID to update an existing glossary. Leave empty to create a new one.")]
        [DataSource(typeof(TermbaseDataHandler))]
        public string TbId { get; set; }
        
        [Display("Allow add new languages", Description = "Allow adding new languages during the update.")]
        [StaticDataSource(typeof(YesNoDataHandler))]
        public bool? AllowAddNewLanguages { get; set; }

        [Display("Overwrite entries with same ID", Description = "Overwrite entries with the same ID during the update.")]
        [StaticDataSource(typeof(YesNoDataHandler))]
        public bool? OverwriteEntriesWithSameId { get; set; }

        [Display("Delimeter")]
        public string? Delimiter { get; set; }
    }
}
