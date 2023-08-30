using Blackbird.Applications.Sdk.Common;
using MQS.ServerProject;

namespace Apps.Memoq.Models.Dto;

public class ProjectDto
{
    [Display("GUID")]
    public string Guid { get; set; }
    public string Name { get; set; }

    public string? Description { get; set; }

    public string? Client { get; set; }

    public string? Domain { get; set; }

    public string? Subject { get; set; }

    public DateTime Deadline { get; set; }

    [Display("Last time changed")]
    public DateTime LastChanged { get; set; }

    [Display("Source language code")]
    public string SourceLanguageCode { get; set; }

    [Display("Target language codes")]
    public List<string> TargetLanguageCodes { get; set; }

    public ProjectDto(ServerProjectInfo project)
    {
        Guid = project.ServerProjectGuid.ToString();
        Name = project.Name;
        Description = project.Description;
        Client = project.Client;
        Domain = project.Domain;
        Subject = project.Subject;
        Deadline = project.Deadline;
        LastChanged = project.LastChanged;
        SourceLanguageCode = project.SourceLanguageCode;
        TargetLanguageCodes = project.TargetLanguageCodes.ToList();
    }
}