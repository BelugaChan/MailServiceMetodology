using MailServiceMetodology.Abstract;
using MailServiceMetodology.Models;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MailServiceMetodology.Services
{
    public class ResetPasswordEmailService : EMailServiceBase<RabbitMqResetPasswordConsumer, string>
    {
        private readonly SmtpOptions options;
        public ResetPasswordEmailService(IOptions<SmtpOptions> options) : base(options)
        {
            this.options = options.Value;
        }

        protected override string HtmlPath => "D:\\MailServiceMetodology\\Templates";

        public async override Task<MimeMessage> GenerateMailContent(RabbitMqResetPasswordConsumer consumer)
        {
            using MimeMessage mailMessage = new MimeMessage();

            mailMessage.From.Add(new MailboxAddress("Publisher", options.From));
            mailMessage.To.Add(new MailboxAddress("test", consumer.Email));
            mailMessage.Subject = "Reset link!";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = await GetHtmlTemplate(consumer.Link, "mailTwoFa.html")
            };
            mailMessage.Body = bodyBuilder.ToMessageBody();

            return mailMessage;
        }

        public async override Task<string> GetHtmlTemplate(string resetLink, string htmlTemplateName)
        {
            string path = Path.Combine(HtmlPath, htmlTemplateName);
            if (!File.Exists(path))
                throw new FileNotFoundException($"Email template not found at {path}");

            string res = await File.ReadAllTextAsync(path);
            return res.Replace("{reset_link}", resetLink);
        }
    }
}
