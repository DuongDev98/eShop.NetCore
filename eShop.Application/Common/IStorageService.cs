namespace eShop.Application.Common
{
    public interface IStorageService
    {
        string GetFileUrl(string fileName);

        string GetFilePath(string fileName);

        Task SaveFileAsync(Stream mediaBinaryStream, string fileName);

        Task DeleteFileAsync(string fileName);
    }
}
