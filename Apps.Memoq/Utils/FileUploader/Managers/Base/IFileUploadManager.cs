namespace Apps.Memoq.Utils.FileUploader.Managers.Base;

public interface IFileUploadManager
{
    Guid BeginChunkedUpload(string id);
    void AddNextChunk(Guid id, byte[] data);
    void EndChunkedUpload(Guid id);
}