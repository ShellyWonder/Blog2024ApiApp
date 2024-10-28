using Blog2024Api.Services.Interfaces;

namespace Blog2024Api.Models
{
    public partial class Blog: IImageEntity
    {
        // These properties are already defined in Blog.cs.
        // Fulfills the interface requirements without duplication.
        public byte[]? ImageData { get; set; }
        public string? ImageType { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
