using MailServiceMetodology.Interfaces;
using MailServiceMetodology.Models;
using Microsoft.Extensions.Options;
using RabbitMqListener.Abstract;
using RabbitMqModel.Models;

namespace MailServiceMetodology.Listeners
{
    public class ResetPasswordRabbitListener : RabbitMqListenerBase<RabbitMqResetPasswordConsumer>
    {
        private readonly IEmailServiceBase<RabbitMqResetPasswordConsumer, string> emailService;
        public ResetPasswordRabbitListener(IOptions<RabbitMqOptions> options, IEmailServiceBase<RabbitMqResetPasswordConsumer, string> emailService) : base(options)
        {
            this.emailService = emailService;
        }

        protected override string QueueName => "ResetPasswordQueue";

        public override async Task ProcessMessageAsync(RabbitMqResetPasswordConsumer message)
        {
            await emailService.SendEmailAsync(message);
        }
    }
}
