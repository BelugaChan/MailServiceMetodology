using MailServiceMetodology.Interfaces;
using MailServiceMetodology.Models;
using Microsoft.Extensions.Options;
using RabbitMqListener.Abstract;
using RabbitMqModel.Models;
using System.Text.Json;

namespace MailServiceMetodology.Listeners
{
    public class TwoFaRabbitListener : RabbitMqListenerBase
    {
        private readonly IEmailServiceBase<RabbitMqTwoFaConsumer,string> emailService;
        public TwoFaRabbitListener(IOptions<RabbitMqOptions> options, IEmailServiceBase<RabbitMqTwoFaConsumer, string> emailService) : base(options)
        {
            this.emailService = emailService;
        }

        protected override string QueueName => "TwoFaQueue";

        public override async Task ProcessMessageAsync(string message)
        {
            var messagedeserialized = JsonSerializer.Deserialize<RabbitMqTwoFaConsumer>(message);
            if(messagedeserialized is null)
            {
                Console.WriteLine("Yeah, that message wasn't nack and put in dead letter queue. It was just vanished..Sorry..");
                return;
            }
            await emailService.SendEmailAsync(messagedeserialized);
        }
    }
}
