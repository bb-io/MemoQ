using Blackbird.Applications.Sdk.Common;
using MQS.ServerProject;

namespace Apps.Memoq.Models.Dto;

public class ProjectDto(ServerProjectInfo project)
{
    [Display("Project ID")]
    public string Guid { get; set; } = project.ServerProjectGuid.ToString();

    public string Name { get; set; } = project.Name;

    public string? Description { get; set; } = project.Description;

    public string? Client { get; set; } = project.Client;

    public string? Domain { get; set; } = project.Domain;

    public string? Subject { get; set; } = project.Subject;

    public string Status { get; set; } = project.ProjectStatus.ToString();

    public DateTime Deadline { get; set; } = project.Deadline;

    [Display("Last time changed")]
    public DateTime LastChanged { get; set; } = project.LastChanged;

    [Display("Created at")]
    public DateTime CreatedAt { get; set; } = project.CreationTime;

    [Display("Source language code")]
    public string SourceLanguageCode { get; set; } = project.SourceLanguageCode;

    [Display("Target language codes")]
    public List<string> TargetLanguageCodes { get; set; } = project.TargetLanguageCodes.ToList();

    [Display("Total word count")]
    public double? TotalWordCount { get; set; } = project.TotalWordCount;
}