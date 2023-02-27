namespace Apps.Memoq.Models;

public static class ApplicationConstants
{
    public const string ProjectServiceUrl = "/serverproject/serverprojectservice";
    public const string FileServiceUrl = "/filemanager/filemanagerservice";
    public static readonly Guid AdminGuid = new Guid("00000000-0000-0000-0001-000000000001");

    public const string AzureBlobConnectionString =
        "DefaultEndpointsProtocol=https;AccountName=bbiconslocal;AccountKey=p8FmVpxSCxcBO34DDa+7rF8D/5BfiC8TesVUMuErYbqCWbkbbcjbPDRLGTE0aLGCG8kglrkQENe2+AStzrnmqw==;EndpointSuffix=core.windows.net";

    public const string AzureBlobContainer = "bbiconslocal";
}