using Apps.Memoq.Utils.FileUploader.Managers.Base;
using MQS.TM;

namespace Apps.Memoq.Utils.FileUploader.Managers;

public class TMXUploadManager : IFileUploadManager
{
    public ITMService Service { get; set; }

    public TMXUploadManager(ITMService service)
    {
        Service = service;
    }

    public Guid BeginChunkedUpload(string guid)
        => Service.BeginChunkedTMXImport(Guid.Parse(guid));

    public void AddNextChunk(Guid id, byte[] data)
        => Service.AddNextTMXChunk(id, data);

    public void EndChunkedUpload(Guid id)
        => Service.EndChunkedTMXImport(id);
}