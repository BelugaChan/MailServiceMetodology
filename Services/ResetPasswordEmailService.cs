using MailServiceMetodology.Abstract;
using MailServiceMetodology.Models;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MailServiceMetodology.Services
{
    public class ResetPasswordEmailService : EMailServiceBase<RabbitMqResetPasswordConsumer, string>
    {
        private readonly SmtpOptions options;
        private readonly IHostEnvironment env;
        public ResetPasswordEmailService(IOptions<SmtpOptions> options, IHostEnvironment env) : base(options)
        {
            this.options = options.Value;
            this.env = env;
        }

        protected override string HtmlPath => Path.Combine(env.ContentRootPath, "Templates");

        public async override Task<MimeMessage> GenerateMailContent(RabbitMqResetPasswordConsumer consumer)
        {
            using MimeMessage mailMessage = new MimeMessage();

            mailMessage.From.Add(new MailboxAddress("Publisher", options.From));
            mailMessage.To.Add(new MailboxAddress("test", consumer.Email));
            mailMessage.Subject = "Reset link!";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = await GetHtmlTemplate(consumer.Link, "mailResetPassword.html")
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
