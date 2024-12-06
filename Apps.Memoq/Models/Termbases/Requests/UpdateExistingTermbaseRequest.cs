using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Memoq.DataSourceHandlers;
using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.MemoQ.Models.Termbases.Requests
{
    public class UpdateExistingTermbaseRequest
    {
        [Display("Termbase ID")]
        [DataSource(typeof(TermbaseDataHandler))]
        public string Id {  get; set; }

        [Display("Allow add new languages")]
        [DataSource(typeof(YesNoDataHandler))]
        public bool? AllowAddNewLanguages {  get; set; }

        [Display("Overwrite enties with same ID")]
        [DataSource(typeof(YesNoDataHandler))]
        public bool? OverwriteEntiesWithSameId {  get; set; }
    }
}
