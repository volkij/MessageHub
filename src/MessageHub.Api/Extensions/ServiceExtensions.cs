using Asp.Versioning;
using FluentValidation;
using FluentValidation.AspNetCore;
using MessageHub.Core.Abstraction.Interfaces;
using MessageHub.Core.Config;
using MessageHub.Core.Validations;
using MessageHub.Core.Validators;
using MessageHub.Domain.Validators;
using MessageHub.Infrastructure.Database;
using MessageHub.Infrastructure.Repositories;
using MessageHub.Services;
using MessageHub.Services.Processing;
using MessageHub.Services.Telemetry;
using MessageHub.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace MessageHub.Api.Extensions
{
    internal static class ServiceExtensions
    {
        internal static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            string databaseConnectionString = configuration.GetConnectionString("Database")!;

            services.AddDbContext<MessageDbContext>(options =>
                options
                    .UseNpgsql(
                        databaseConnectionString,
                        npgsqlOptions => npgsqlOptions
                            .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Messages)));
        }

        public static void ConfigureOptions(this IServiceCollection services)
        {
            services.AddScoped<IValidator<AccountConfig>, AccountConfigValidator>();
            services.AddScoped<IValidator<MessageHubConfig>, MessageHubConfigValidator>();
            services.AddScoped<IValidator<List<SenderConfig>>, SenderConfigValidator>();

            //services.AddOptions<>().BindConfiguration("Senders");

            services.AddOptions<List<SenderConfig>>()
                .BindConfiguration("Senders")
                .ValidateFluentValidation()
                .ValidateOnStart();

            services.AddOptions<AccountConfig>()
                .BindConfiguration("Accounts")
                .ValidateFluentValidation()
                .ValidateOnStart();

            services.AddOptions<MessageHubConfig>()
                .BindConfiguration("MessageHub")
                .ValidateFluentValidation()
                .ValidateOnStart();
        }


        public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            //https://medium.com/@jeslurrahman/implementing-health-checks-in-net-8-c3ba10af83c3
            services.AddHealthChecks()
                .AddNpgSql(configuration.GetConnectionString("Database"))
                .AddCheck<MemoryHealthCheck>($"Memory Check", failureStatus: HealthStatus.Unhealthy, tags: new[] { "Feedback Service" });

            //services.AddHealthChecksUI();
            services.AddHealthChecksUI(opt =>
            {
                opt.SetEvaluationTimeInSeconds(10);
                opt.MaximumHistoryEntriesPerEndpoint(60);
                opt.SetApiMaxActiveRequests(1);
                opt.AddHealthCheckEndpoint("MessageHubAPI", "http://localhost:8080/health");

            }).AddInMemoryStorage();
        }

        public static void ConfigureValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<CreateMessageRequestValidator>();
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<SenderConfigurationService>();
            services.AddScoped<ISenderService, SenderService>();
            services.AddSingleton<IProviderFactory, ProviderFactory>();
            services.AddScoped<IMessageRequestService, MessageRequestService>();
            services.AddScoped<UnitOfWork>();
            services.AddScoped<MessageService>();
            services.AddScoped<TemplateService>();
            services.AddScoped<TemplateDownloadService>();
            services.AddScoped<PushTokenService>();

            services.AddScoped<IMessageProcessingService, EmailProcessingService>();
            services.AddScoped<IMessageProcessingService, SmsProcessingService>();
            services.AddScoped<IMessageProcessingService, PushProcessingService>();
        }

        public static void ConfigureOpenTelemetry(this IServiceCollection services)
        {
            services.AddSingleton<MessageHubMetrics>();

            services.AddOpenTelemetry()
             .WithMetrics(builder =>
             {
                 builder
                     .ConfigureResource(resource => resource.AddService("MessageHub"))
                     .AddMeter("MessageHubMeter")
                     .AddAspNetCoreInstrumentation()
                     .AddHttpClientInstrumentation()
                     .AddRuntimeInstrumentation()
                     .AddPrometheusExporter();
             });
        }

        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version"));
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
        }
    }
}