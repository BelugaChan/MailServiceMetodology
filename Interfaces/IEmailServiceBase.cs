using MimeKit;

namespace MailServiceMetodology.Interfaces
{
    public interface IEmailServiceBase<T,U> 
        where T : class
        where U : class
    {
        Task SendEmailAsync(T consumer);
        Task<MimeMessage> GenerateMailContent(T consumer);

        Task<string> GetHtmlTemplate(U data, string htmlTemplateName);
    }
}
