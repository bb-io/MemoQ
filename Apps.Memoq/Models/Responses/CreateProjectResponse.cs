namespace Apps.Memoq.Models.Responses;

public class CreateProjectResponse
{
    public string ProjectGuid { get; set; }

    public string ProjectDeadline { get; set; }

    public string SourceLanguage { get; set; }

    public IEnumerable<Language> TargetLanguages { get; set; }
}