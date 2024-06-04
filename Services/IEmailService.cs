using EmailServiceAPI.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace EmailServiceAPI.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailService(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Sender Name", _emailConfig.Username));
                email.To.Add(new MailboxAddress("", to));
                email.Subject = subject;
                email.Body = new TextPart("plain") { Text = body };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_emailConfig.SmtpServer, Int32.Parse(_emailConfig.Port), false);
                await smtp.AuthenticateAsync(_emailConfig.Username, _emailConfig.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                //test comment
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}