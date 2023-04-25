﻿namespace Apps.Memoq.Models.Requests;

public class CreateProjectTemplateRequest
{
    public string SourceLangCode { get; set; }
    
    public string TargetLangCodes { get; set; }

    public string ProjectName { get; set; }

    public string TemplateGuid { get; set; }
}