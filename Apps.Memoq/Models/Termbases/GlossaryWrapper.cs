using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Memoq.Models.Termbases;

public class GlossaryWrapper
{
    [Display("Glossary file", Description = "The glossary file to upload and process.")]
    public FileReference Glossary { get; set; }
}