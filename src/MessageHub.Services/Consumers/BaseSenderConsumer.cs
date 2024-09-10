﻿using MassTransit;
using MessageHub.Core.Abstraction.Interfaces;
using Microsoft.Extensions.Logging;

namespace MessageHub.Services.Consumers
{
    public abstract class BaseSenderConsumer<TEvent>(ILogger<BaseSenderConsumer<TEvent>> logger, ISenderService senderService)
        : BaseConsumer(logger), IConsumer<TEvent>
        where TEvent : class
    {
        protected readonly ISenderService SenderService = senderService;

        public Task Consume(ConsumeContext<TEvent> context)
        {
            try
            {
                Logger.LogInformation($"Received {typeof(TEvent).Name}: {context.MessageId}");

                SendMessage(context);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error when consuming a {typeof(TEvent).Name}");
            }

            return Task.CompletedTask;
        }

        protected abstract void SendMessage(ConsumeContext<TEvent> context);
    }
}