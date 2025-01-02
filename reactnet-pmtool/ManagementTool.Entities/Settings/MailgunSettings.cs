namespace ManagementTool.Entities.Settings;

public class MailgunSettings
    {
        public string ApiKey { get; set; }
        public string Domain { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
    }