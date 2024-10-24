namespace Blog2024ApiApp.ViewModels
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        // Added property to store the custom error message
        public string? CustomErrorMessage { get; set; }
    }
}
