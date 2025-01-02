// ManagementTool.Infrastructure
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ManagementTool.Core.Services.Messaging;
using ManagementTool.Entities.Settings;
using Microsoft.Extensions.Options;

namespace ManagementTool.Infrastructure.Messaging;

public class EmailService : IEmailService
{
    private readonly MailgunSettings _mailgunSettings;

        public EmailService(IOptions<MailgunSettings> mailgunSettings)
        {
            _mailgunSettings = mailgunSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using var client = new HttpClient();
            var requestContent = new StringContent(
                $"from={_mailgunSettings.FromName} <{_mailgunSettings.FromEmail}>&to={toEmail}&subject={subject}&html={body}",
                Encoding.UTF8,
                "application/x-www-form-urlencoded");
            
            var requestUri = $"https://api.mailgun.net/v3/{_mailgunSettings.Domain}/messages";
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes($"api:{_mailgunSettings.ApiKey}")));
            
            var response = await client.PostAsync(requestUri, requestContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Email sent successfully.");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to send email. Status Code: {response.StatusCode}, Error: {error}");
            }

        }
}