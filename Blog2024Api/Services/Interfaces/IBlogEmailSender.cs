using Microsoft.AspNetCore.Identity.UI.Services;

namespace Blog2024Api.Services.Interfaces
{
    public interface IBlogEmailSender : IEmailSender
    {
        Task SendContactEmailAsync(string emailFrom, string name, string subject, string htmlMessage);
    }
}
