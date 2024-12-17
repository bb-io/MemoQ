using Apps.Memoq.DataSourceHandlers;
using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.MemoQ.Models.Termbases.Requests
{
    public class UpdateExistingTermbaseRequest
    {
        [Display("Termbase ID")]
        [DataSource(typeof(TermbaseDataHandler))]
        public string Id { get; set; }

        [Display("Allow add new languages")]
        [StaticDataSource(typeof(YesNoDataHandler))]
        public bool? AllowAddNewLanguages { get; set; }

        [Display("Overwrite enties with same ID")]
        [StaticDataSource(typeof(YesNoDataHandler))]
        public bool? OverwriteEntiesWithSameId { get; set; }
    }
}
