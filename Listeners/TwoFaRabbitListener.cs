using MailServiceMetodology.Interfaces;
using MailServiceMetodology.Models;
using Microsoft.Extensions.Options;
using RabbitMqListener.Abstract;
using RabbitMqModel.Models;

namespace MailServiceMetodology.Listeners
{
    public class TwoFaRabbitListener : RabbitMqListenerBase<RabbitMqTwoFaConsumer>
    {
        private readonly IEmailServiceBase<RabbitMqTwoFaConsumer,string> emailService;
        public TwoFaRabbitListener(IOptions<RabbitMqOptions> options, IEmailServiceBase<RabbitMqTwoFaConsumer, string> emailService) : base(options)
        {
            this.emailService = emailService;
        }

        protected override string QueueName => "TwoFaQueue";

        public override async Task ProcessMessageAsync(RabbitMqTwoFaConsumer message)
        {
            await emailService.SendEmailAsync(message);
        }
    }
}
