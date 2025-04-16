using MailKit.Net.Smtp;
using MailKit.Security;
using MailServiceMetodology.Interfaces;
using MailServiceMetodology.Models;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MailServiceMetodology.Abstract
{
    public abstract class EMailServiceBase<T,U> : IEmailServiceBase<T,U>
        where T : class
        where U : class
    {
        private readonly SmtpOptions options;
        protected abstract string HtmlPath { get; }
        protected EMailServiceBase(IOptions<SmtpOptions> options)
        {
            this.options = options.Value;
        }
        public async Task SendEmailAsync(T consumer)
        {
            using SmtpClient smtpClient = new SmtpClient();
            try
            {
                await smtpClient.ConnectAsync(options.Server, options.Port, SecureSocketOptions.SslOnConnect);

                await smtpClient.AuthenticateAsync(
                    userName: options.From,
                    password: options.Password
                );

                var mailMessage = await GenerateMailContent(consumer);

                await smtpClient.SendAsync(mailMessage);
                Console.WriteLine("Message was send successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                await smtpClient.DisconnectAsync(true);
            }
        }

        public abstract Task<MimeMessage> GenerateMailContent(T consumer);

        public abstract Task<string> GetHtmlTemplate(U data, string htmlTemplateName);
    }
}
