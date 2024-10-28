namespace Blog2024Api.Services.Interfaces
{
    public interface IImageEntity
    {
            IFormFile? ImageFile { get; set; }
            byte[]? ImageData { get; set; }
            string? ImageType { get; set; }
    }
}
