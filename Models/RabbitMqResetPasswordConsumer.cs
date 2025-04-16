namespace MailServiceMetodology.Models
{
    public class RabbitMqResetPasswordConsumer
    {
        public required string Link {  get; set; }

        public required string Email { get; set; }
    }
}
