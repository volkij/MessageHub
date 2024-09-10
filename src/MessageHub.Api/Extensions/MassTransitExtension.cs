using MessageHub.Core.Config;
using MessageHub.Domain.Events;
using MessageHub.Infrastructure.ServiceBus;
using MessageHub.Services.Consumers;
using RabbitMQ.Client;

namespace MessageHub.Api.Extensions
{
    internal static class MassTransitExtension
    {
        internal static void AddMassTransitRabbit(this IServiceCollection services, IConfiguration configuration)
        {
            var massTransitConfig = configuration.GetSection("MassTransit").Get<MassTransitConfig>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<MessageCreateConsumer>();
                x.AddConsumer<EmailConsumer>();
                x.AddConsumer<SmsConsumer>();
                x.AddConsumer<PushConsumer>();
                x.AddConsumer<ProviderResultConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(massTransitConfig.Host, h =>
                    {
                        h.Username(massTransitConfig.Username);
                        h.Password(massTransitConfig.Password);
                    });

                    ConfigureEndpoint<MessageCreateEvent, MessageCreateConsumer>(cfg, context, Exchanges.Messages, Exchanges.MessagesNew, RoutingKeys.New);
                    ConfigureEndpoint<EmailQueuedEvent, EmailConsumer>(cfg, context, Exchanges.Emails, Exchanges.EmailsQueued, RoutingKeys.New);
                    ConfigureEndpoint<SmsQueuedEvent, SmsConsumer>(cfg, context, Exchanges.Sms, Exchanges.SmsQueued, RoutingKeys.New);
                    ConfigureEndpoint<PushQueuedEvent, PushConsumer>(cfg, context, Exchanges.Push, Exchanges.PushQueued, RoutingKeys.New);
                    ConfigureEndpoint<ProviderResultEvent, ProviderResultConsumer>(cfg, context, Exchanges.ProviderResult, Exchanges.ProviderResultNew, RoutingKeys.New);

                    cfg.ConfigureJsonSerializerOptions(options =>
                    {
                        options.IncludeFields = true;
                        return options;
                    });
                });
            });
        }

        private static void ConfigureEndpoint<TEvent, TConsumer>(IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext context, string exchangeName, string endpointName, string routingKey)
            where TEvent : class
            where TConsumer : class, IConsumer<TEvent>
        {
            cfg.Message<TEvent>(x => x.SetEntityName(exchangeName));
            cfg.Publish<TEvent>(x => x.ExchangeType = ExchangeType.Direct);

            cfg.ReceiveEndpoint(endpointName, e =>
            {
                e.ConfigureConsumeTopology = false;
                e.ConfigureConsumer<TConsumer>(context);
                e.Bind(exchangeName, s =>
                {
                    s.RoutingKey = routingKey;
                    s.ExchangeType = ExchangeType.Direct;
                });
            });
        }
    }
}