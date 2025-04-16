namespace MailServiceMetodology.Models
{
    public class RabbitMqTwoFaConsumer
    {
        public required Guid Id { get; set; }

        public required string Code { get; set; }

        public required string Email { get; set; }

        public static RabbitMqTwoFaConsumer Create(Guid id, string code, string email)
            => new RabbitMqTwoFaConsumer()
            {
                Id = id,
                Code = code,
                Email = email
            };

        
    }
}
