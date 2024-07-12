using Blackbird.Applications.Sdk.Common.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.MemoQ.DataSourceHandlers.EnumDataHandlers
{
    public class PreviewCreationDataHandler : IStaticDataSourceHandler
    {
        protected Dictionary<string, string> EnumValues => new()
        {
            {"2", "Create preview"},
            {"1", "Disable preview creation"},
            {"0", "Determined by project settings" }
        };

        public Dictionary<string, string> GetData()
        {
            return EnumValues;
        }
    }
}
