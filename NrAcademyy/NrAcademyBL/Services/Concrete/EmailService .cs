using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using NrAcademyBL.Configuration;
using NrAcademyBL.DTOs;
using NrAcademyBL.Services.Abstract;

namespace NrAcademyBL.Services.Concrete
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        // ✅ IOptions ile inject et
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task<bool> SendEmailAsync(EmailDto email)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
                message.To.Add(new MailboxAddress("", email.To));
                message.Subject = email.Subject;

                var bodyBuilder = new BodyBuilder { HtmlBody = email.Body };
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, _emailSettings.EnableSSL);
                    await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendVerificationEmailAsync(string email, string verificationCode)
        {
            var htmlBody = $@"
                <html>
                    <body style='font-family: Arial, sans-serif;'>
                        <h2>Email Doğrulama</h2>
                        <p>Salam,</p>
                        <p>Emailinizi doğrulamak üçün kodu istifadə edin:</p>
                        <h3 style='color: #007bff;'>{verificationCode}</h3>
                        <p>Bu kod 15 dəqiqə müddətinə etibarlı olacaq.</p>
                        <br>
                        <p>NR Academy Team</p>
                    </body>
                </html>";

            var emailDto = new EmailDto
            {
                To = email,
                Subject = "Email Doğrulama Kodu",
                Body = htmlBody
            };

            return await SendEmailAsync(emailDto);
        }
    }
}