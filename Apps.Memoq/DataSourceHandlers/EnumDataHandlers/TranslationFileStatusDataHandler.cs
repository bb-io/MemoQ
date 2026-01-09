using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.MemoQ.DataSourceHandlers.EnumDataHandlers
{
    public class TranslationFileStatusDataHandler : IStaticDataSourceItemHandler
    {
        protected Dictionary<string, string> EnumValues => new()
    {
        { "TranslationInProgress", "Translation in progress" },
        { "TranslationFinished", "Translation finished" },
        { "ProofreadingFinished", "Proofreading finished" }
    };

        public IEnumerable<DataSourceItem> GetData()
            => EnumValues.Select(x => new DataSourceItem(x.Key, x.Value));
    }
}
