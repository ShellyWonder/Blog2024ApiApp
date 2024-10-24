namespace Blog2024ApiApp.DTO
{
    public class MailSettingsDTO
    {
        //Configuration & use of smtp gmail server
        public string? Mail {  get; set; }
        public string? DisplayName { get; set; }
        public string? Password { get; set; }
        public string? Host { get; set; }
        public int Port { get; set; }
    }
}
