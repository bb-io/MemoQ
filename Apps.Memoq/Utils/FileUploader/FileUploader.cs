using Apps.Memoq.Utils.FileUploader.Managers.Base;

namespace Apps.Memoq.Utils.FileUploader;

public static class FileUploader
{
    public static Guid UploadFile(byte[] file, IFileUploadManager manager, string id)
    {
        const int chunkSize = 500000;
        byte[] chunkBytes = new byte[chunkSize];
        int bytesRead;

        using var fileStream = new MemoryStream(file);
        var guid = manager.BeginChunkedUpload(id);
        while ((bytesRead = fileStream.Read(chunkBytes, 0, chunkSize)) != 0)
        {
            byte[] dataToUpload;
            if (bytesRead == chunkSize)
                dataToUpload = chunkBytes;
            else
            {
                dataToUpload = new byte[bytesRead];
                Array.Copy(chunkBytes, dataToUpload, bytesRead);
            }

            manager.AddNextChunk(guid, dataToUpload);
        }

        if (guid != Guid.Empty)
            manager.EndChunkedUpload(guid);

        return guid;
    }
}