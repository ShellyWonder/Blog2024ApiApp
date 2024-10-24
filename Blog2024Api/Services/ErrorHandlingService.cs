using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Blog2024ApiApp.Areas.Identity.Pages;
using Blog2024ApiApp.Services.Interfaces;

namespace Blog2024ApiApp.Services
{
    public class ErrorHandlingService : IErrorHandlingService
    {
        #region HANDLE ERROR
        public ViewResult HandleError(string errorMessage)
        {
            var errorModel = new ErrorModel
            {
                CustomErrorMessage = errorMessage
            };

            // Manually construct and return a ViewResult with the error model
            return new ViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary<ErrorModel>(
                    new EmptyModelMetadataProvider(),
                    new ModelStateDictionary())
                {
                    Model = errorModel
                }
            };
        } 
        #endregion
    }
}
