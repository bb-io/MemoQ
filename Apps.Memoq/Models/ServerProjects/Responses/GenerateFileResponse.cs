using System.Net;

namespace Apps.Memoq.Models.ServerProjects.Responses;

public class GenerateFileResponse
{
    public HttpStatusCode StatusCode { get; set; }

    public string[] Files { get; set; }

    public int Total { get; set; }
}