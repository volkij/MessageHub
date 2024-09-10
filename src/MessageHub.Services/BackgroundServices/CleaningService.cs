using MessageHub.Core.Config;
using MessageHub.Domain.Entities;
using MessageHub.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace MessageHub.Services.BackgroundServices
{
    /// <summary>
    /// Service that cleans up old messages
    /// </summary>
    public class CleaningService(ILogger<CleaningService> logger, IServiceProvider serviceProvider, IOptions<MessageHubConfig> config) : IHostedService, IDisposable
    {
        ILogger<CleaningService> Logger = logger;
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly MessageHubConfig _config = config.Value;
        private Timer _timer;


        public Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("CleaningService running.");
            _timer = new Timer(ExecuteCleaningTask, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private async void ExecuteCleaningTask(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<UnitOfWork>();

                var stopwatch = Stopwatch.StartNew();
                List<Message> messagesToDelete = await unitOfWork.MessageRepository.GetMessagesOlderThan(DateTime.Now.AddDays(_config.MessageRetentionDays * -1).ToUniversalTime());

                foreach (var message in messagesToDelete)
                {
                    unitOfWork.MessageRepository.Delete(message);
                }

                await unitOfWork.SaveChangesAsync();
                stopwatch.Stop();

                Logger.LogInformation("{0} messages deleted in {1} milliseconds", messagesToDelete.Count, stopwatch.Elapsed.TotalMilliseconds);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("CleaningService is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}