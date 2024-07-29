using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.ServerProjects.Requests;

public class UpdateProjectRequest
{
    public DateTime? Deadline { get; set; }

    [Display("Callback URL")] public string? CallbackUrl { get; set; }

    [Display("Client")] public string? Client { get; set; }

    [Display("Custom metadata")] public string? CustomMetas { get; set; }

    [Display("Description")] public string? Description { get; set; }

    [Display("Domain")] public string? Domain { get; set; }

    [Display("Subject")] public string? Subject { get; set; }
}