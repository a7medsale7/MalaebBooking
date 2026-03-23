using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.Mail
{
    public class EmailService : IEmailSender
    {
        private readonly MailSetting mailSettings;
        private readonly ILogger<EmailService> logger;

        // ----------------------
        // Constructor
        // ----------------------
        public EmailService(IOptions<MailSetting> options, ILogger<EmailService> logger)
        {
            mailSettings = options.Value;
            this.logger = logger;
        }

        // ----------------------
        // Send Email Async
        // ----------------------
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try 
            {
                var message = new MimeMessage();

                // From
                if (!string.IsNullOrEmpty(mailSettings.DisplayName))
                    message.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
                else
                    message.From.Add(MailboxAddress.Parse(mailSettings.Mail));

                // To
                message.To.Add(MailboxAddress.Parse(email));

                // Subject
                message.Subject = subject;

                // Body
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlMessage
                };
                message.Body = bodyBuilder.ToMessageBody();

                using var smtp = new SmtpClient();

                logger.LogInformation("Attempting to send email to {Email} via {Host}:{Port}", email, mailSettings.Host, mailSettings.Port);

                // Connect to SMTP server
                await smtp.ConnectAsync(
                    mailSettings.Host,
                    mailSettings.Port,
                    SecureSocketOptions.SslOnConnect
                );

                // Authenticate
                await smtp.AuthenticateAsync(
                    mailSettings.Mail,
                    mailSettings.Password
                );

                // Send email
                await smtp.SendAsync(message);

                // Disconnect
                await smtp.DisconnectAsync(true);

                logger.LogInformation("Email sent successfully to {Email}", email);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send email to {Email}. Error: {Message}", email, ex.Message);
                throw; // Rethrow so Hangfire knows it failed
            }
        }
    }
}