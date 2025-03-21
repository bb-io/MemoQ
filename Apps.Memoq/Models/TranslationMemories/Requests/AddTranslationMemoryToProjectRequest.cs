﻿using Apps.Memoq.DataSourceHandlers;
using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Memoq.Models.TranslationMemories.Requests;

public class AddTranslationMemoryToProjectRequest
{
    [Display("Translation memory ID"), DataSource(typeof(TranslationMemoryDataHandler))]
    public IEnumerable<string>? TmGuids { get; set; }

    [Display("Target language code"), StaticDataSource(typeof(TargetLanguageDataHandler))]
    public string TargetLanguageCode { get; set; }

    [Display("Master translation memory ID"), DataSource(typeof(TranslationMemoryDataHandler))]
    public string? MasterTmGuid { get; set; }

    [Display("Primary translation memory ID"), DataSource(typeof(TranslationMemoryDataHandler))]
    public string? PrimaryTmGuid { get; set; }
}