using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.MemoQ.Events.Polling.Models
{
    public class TranslationFileStatus
    {
        [StaticDataSource(typeof(TranslationFileStatusDataHandler))]
        public string Status { get; set; }
    }
}
