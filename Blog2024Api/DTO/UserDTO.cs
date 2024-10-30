namespace Blog2024ApiApp.DTO
{
    public class UserDTO
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? GitHubUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public byte[]? ImageData { get; set; }
        public string? ImageType { get; set; }
    }
}
