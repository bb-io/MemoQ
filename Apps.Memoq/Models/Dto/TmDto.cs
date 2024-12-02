using Blackbird.Applications.Sdk.Common;
using MQS.TM;

namespace Apps.Memoq.Models.Dto;

public class TmDto
{
    [Display("TM GUID")]
    public string Guid { get; set; }
    
    public string Client { get; set; }

    public string Domain { get; set; }

    public string Subject { get; set; }

    [Display("Last modified")]
    public DateTime LastModified { get; set; }

    public string Project { get; set; }

    [Display("Number of entries")]
    public int NumOfEntries { get; set; }

    [Display("Source language code")]
    public string SourceLanguageCode { get; set; }

    [Display("Target language code")]
    public string TargetLanguageCode { get; set; }

    [Display("TM engine type")]
    public string TmEngineType { get; set; }
    
    [Display("Optimization preference")]
    public string OptimizationPreference { get; set; }

    public TmDto(TMInfo tm)
    {
        Guid = tm.Guid.ToString();
        Client = tm.Client;
        Domain = tm.Domain;
        Subject = tm.Subject;
        LastModified = tm.LastModified;
        Project = tm.Project;
        NumOfEntries = tm.NumOfEntries;
        SourceLanguageCode = tm.SourceLanguageCode;
        TargetLanguageCode = tm.TargetLanguageCode;
        TmEngineType = tm.TMEngineType.ToString();
        OptimizationPreference = tm.OptimizationPreference.ToString();
    }
}