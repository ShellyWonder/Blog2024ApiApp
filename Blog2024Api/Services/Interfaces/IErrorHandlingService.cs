using Microsoft.AspNetCore.Mvc;
using Blog2024ApiApp.Areas.Identity.Pages;

namespace Blog2024ApiApp.Services.Interfaces
{
    public interface IErrorHandlingService
    {
        ViewResult HandleError(string errorMessage);
    }
}
