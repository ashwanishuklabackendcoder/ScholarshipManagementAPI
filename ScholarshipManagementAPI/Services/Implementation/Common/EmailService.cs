using Microsoft.Extensions.Options;
using ScholarshipManagementAPI.DTOs.Common.Settings;
using ScholarshipManagementAPI.Services.Interface.Common;
using System.Net;
using System.Net.Mail;

namespace ScholarshipManagementAPI.Services.Implementation.Common
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettingsDto _settings;

        public EmailService(IOptions<SmtpSettingsDto> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            using var client = CreateClient();
            using var message = CreateMessage(to, subject, body, false);
            await client.SendMailAsync(message);
        }

        public async Task SendHtmlAsync(string to, string subject, string htmlBody)
        {
            using var client = CreateClient();
            using var message = CreateMessage(to, subject, htmlBody, true);
            await client.SendMailAsync(message);
        }




        // helper methods
        private SmtpClient CreateClient()
        {
            return new SmtpClient(_settings.Host, _settings.Port)
            {
                EnableSsl = _settings.EnableSsl,
                Credentials = new NetworkCredential(
                    _settings.UserName,
                    _settings.Password)
            };
        }

        private MailMessage CreateMessage(string to, string subject, string body, bool isHtml)
        {
            return new MailMessage
            {
                From = new MailAddress(_settings.FromEmail, _settings.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml,
                To = { to }
            };
        }
    
    
    }
}
