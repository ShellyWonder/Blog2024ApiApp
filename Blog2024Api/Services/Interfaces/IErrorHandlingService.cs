using Microsoft.AspNetCore.Mvc;

namespace Blog2024Api.Services.Interfaces
{
    public interface IErrorHandlingService
    {
        ViewResult HandleError(string errorMessage);
    }
}
