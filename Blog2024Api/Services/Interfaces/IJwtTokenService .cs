using Blog2024Api.Identity;

namespace Blog2024Api.Services.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(ApplicationUser user);
        Task<ApplicationUser?> AuthenticateUserAsync(string username, string password);
    }
}
