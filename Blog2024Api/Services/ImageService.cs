using Blog2024Api.Services.Interfaces;

namespace Blog2024Api.Services
{
    public class ImageService : IImageService
    {
        #region CONVERT FILE TO BYTE ARRAY
        //image delivered to database from the client
        public async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            if (file is null) return null!;
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
        #endregion

        #region CONVERT FILE TO BYTE ARRAY(OVERLOAD)
        //image delivered to database from the client(overload)
        public async Task<byte[]> ConvertFileToByteArrayAsync(string fileName)
        {
            var file = $"{Directory.GetCurrentDirectory()}/wwwroot/img/{fileName}";
            return await File.ReadAllBytesAsync(file);
        }
        #endregion

        #region DECODE IMAGE
        //image delivered from database to the client
        public string? DecodeImage(byte[] data, string type)
        {
            if (data is null || type is null) return null;
            return $"data:image/{type};base64,{Convert.ToBase64String(data)}";

        }
        #endregion

        #region GET FILE TYPE
        public string GetFileType(IFormFile file)
        {
            return file.ContentType;
        }
        #endregion

        #region IMG IMPLEMENTATION
        public async Task<T> SetImageAsync<T>(T entity) where T : IImageEntity
        {
            if (entity.ImageFile != null)
            {    //Convert incoming file into a byte array
                entity.ImageData = await ConvertFileToByteArrayAsync(entity.ImageFile);
                entity.ImageType = entity.ImageFile.ContentType;
            }
            else
            {
                // Assign default image if no image is uploaded
                var defaultImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "default_icon.png");
                entity.ImageData = await File.ReadAllBytesAsync(defaultImagePath);
                entity.ImageType = "image/png";
            }
            return entity;
        }
        #endregion

        #region MANAGE FILE SIZE
        public int Size(IFormFile file)
        {
            return Convert.ToInt32(file?.Length);
        }
        #endregion

    }
}
