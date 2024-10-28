using Microsoft.AspNetCore.Mvc;

namespace Blog2024ApiApp.Services.Interfaces
{
    public interface IErrorHandlingService
    {
        ViewResult HandleError(string errorMessage);
    }
}
