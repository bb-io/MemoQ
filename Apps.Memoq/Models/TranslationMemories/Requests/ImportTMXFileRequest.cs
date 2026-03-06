using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Memoq.Models.TranslationMemories.Requests;

public class ImportTmxFileRequest
{
    [Display("Translation memory ID")]
    public string TmGuid { get; set; }

    public FileReference File { get; set; }

    [Display("Process Trados TMX")]
    public bool? ProcessTradosTmx { get; set; }

    [Display("Import memoQ formatting")]
    public bool? ImportMemoQFormatting { get; set; }

    [Display("Import <ut> as memoQ tag")]
    public bool? ImportUtAsMemoQTag { get; set; }

    [Display("Custom <tags> in text as memoQ tags")]
    public bool? CustomTagsAsMemoQTags { get; set; }
}