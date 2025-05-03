using MailServiceMetodology.Interfaces;
using MailServiceMetodology.Listeners;
using MailServiceMetodology.Models;
using MailServiceMetodology.Services;
using RabbitMqListener.Interfaces;
using RabbitMqModel.Models;
using Microsoft.AspNetCore.Hosting;

namespace MailServiceMetodology
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);


            builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection(nameof(SmtpOptions)));
            builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(nameof(RabbitMqOptions)));
            builder.Services.AddSingleton<IEmailServiceBase<RabbitMqTwoFaConsumer, string>, VerificationCodeEmailService>();
            builder.Services.AddSingleton<IEmailServiceBase<RabbitMqResetPasswordConsumer,string>,ResetPasswordEmailService>();

            builder.Services
                .AddHostedService<TwoFaRabbitListener>()
                .AddSingleton<IRabbitMqListenerBase,TwoFaRabbitListener>();
            builder.Services
                .AddHostedService<ResetPasswordRabbitListener>()
                .AddSingleton<IRabbitMqListenerBase, ResetPasswordRabbitListener>();
           
            var host = builder.Build();

            host.Run();
        }
    }
}
