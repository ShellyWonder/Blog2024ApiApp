using Blog2024Api.Services.Interfaces;

namespace Blog2024Api.Services.Interfaces
{
    public interface IImageService
    {
        Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file);
        Task<byte[]> ConvertFileToByteArrayAsync(string fileName);
        Task<T> SetImageAsync<T>(T entity) where T : IImageEntity;
        string? DecodeImage(byte[] data, string type);

        string GetFileType(IFormFile file);
        int Size(IFormFile file);
    }
}
