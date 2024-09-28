using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Wrappers;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace GHSTShipping.Infrastructure.Resources.Services
{
    public class EmailSender(string clientHost, string smtpServer, int smtpPort, string smtpUser, string smtpPass) : IEmailSender
    {
        private readonly string _clientHost = clientHost;
        private readonly string _smtpServer = smtpServer;
        private readonly int _smtpPort = smtpPort;
        private readonly string _smtpUser = smtpUser;
        private readonly string _smtpPass = smtpPass;

        public async Task<BaseResult> SendEmailSetPasswordAsync(string emailAddress, string fullName, string token)
        {
            string appName = "GHST";
            string emailTemplate = ProjectResources.EmailTemplate.VI_SET_PASSWORD;
            string link = $"{_clientHost}/set-password?token={token}";
            emailTemplate = emailTemplate.Replace("{{userName}}", fullName).Replace("{{link}}", link);

            var result = await SendEmailAsync(appName, emailAddress, $"[{appName}] Xác nhận đăng ký tài khoản", emailTemplate);

            return result;
        }

        public async Task<BaseResult> SendEmailResetPasswordAsync(string emailAddress, string fullName, string token)
        {
            string appName = "GHST";
            string emailTemplate = ProjectResources.EmailTemplate.VI_RESET_PASSWORD;
            string link = $"{_clientHost}/reset-password?token={token}";
            emailTemplate = emailTemplate.Replace("{{userName}}", fullName).Replace("{{link}}", link);

            var result = await SendEmailAsync(appName, emailAddress, $"[{appName}] Yêu cầu đặt lại mật khẩu", emailTemplate);

            return result;
        }

        public async Task<BaseResult> SendEmailAsync(string appName, string toEmail, string subject, string htmlMessage)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(appName, _smtpUser));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            try
            {
                // Connect to the SMTP server
                await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);

                // Authenticate
                await client.AuthenticateAsync(_smtpUser, _smtpPass);

                // Send email
                await client.SendAsync(emailMessage);

                return BaseResult.Ok();
            }
            catch (Exception ex)
            {
                // Handle exceptions (logging, etc.)
                Console.WriteLine($"Failed to send email: {ex.Message}");

                return new Error(ErrorCode.Exception, $"Failed to send email: {ex.Message}");
            }
            finally
            {
                // Disconnect and dispose
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}
