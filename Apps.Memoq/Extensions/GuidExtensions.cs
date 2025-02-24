using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Memoq.Models.Termbases.Requests;
using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.MemoQ.Extensions
{
    public static class GuidExtensions
    {
        public static Guid ParseWithErrorHandling(ReadOnlySpan<char> input)
        {
            Guid guid;
            try
            {
                guid = Guid.Parse(input);
            }
            catch (FormatException)
            {
                throw new PluginMisconfigurationException($"The input {input} is not a valid GUID. Please provide a valid GUID.");
            }
            return guid;
        }
    }
}
