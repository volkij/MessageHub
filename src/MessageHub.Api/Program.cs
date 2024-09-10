using Elastic.Extensions.Logging;
using MessageHub.Services.BackgroundServices;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(builder =>
{
    builder.AddConsole();
    builder.AddElasticsearch();
});

builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.AddMassTransitRabbit(builder.Configuration);

builder.Services.ConfigureOptions();
builder.Services.ConfigureServices();
builder.Services.ConfigureOpenTelemetry();

builder.Services.AddControllers();

builder.Services.ConfigureValidators();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<CleaningService>();

builder.Services.ConfigureApiVersioning();

//builder.Services.ConfigureHealthChecks(builder.Configuration);



var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<ApiKeyMiddleware>();

//HealthCheck Middleware
/*
app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(options =>
{
    options.UIPath = "/healthcheck-ui";
    //options.AddCustomStylesheet("./HealthCheck/Custom.css");
});
*/


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseSwagger();
app.UseSwaggerUI();
app.ApplyMigrations();

//app.UseHttpsRedirection();

app.UseAuthorization();



app.UseStaticFiles();
app.MapControllers();
app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.Run();
