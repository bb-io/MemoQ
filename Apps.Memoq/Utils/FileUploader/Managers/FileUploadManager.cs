using Apps.Memoq.Utils.FileUploader.Managers.Base;
using MQS.FileManager;

namespace Apps.Memoq.Utils.FileUploader.Managers;

public class FileUploadManager : IFileUploadManager
{
    public IFileManagerService Service { get; set; }

    public FileUploadManager(IFileManagerService service)
    {
        Service = service;
    }

    public Guid BeginChunkedUpload(string fileName)
        => Service.BeginChunkedFileUpload(fileName, false);

    public void AddNextChunk(Guid id, byte[] data)
        => Service.AddNextFileChunk(id, data);

    public void EndChunkedUpload(Guid id)
        => Service.EndChunkedFileUpload(id);
}