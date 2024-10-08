# MessageHub

DotNet Core 8 demonstration project. A microservice that handles sending messages to various channels, allowing the use of templates and the creation of custom providers for different types of messages.


The microservice uses the following tools and containers, configured in the Docker Compose project:

- **messagehub-database**: PostgreSQL for data storage
- **messagehub-rabbitmq**: RabbitMQ for internal messaging
- **messagehub-elastic**: Elasticsearch for logging
- **messagehub-kibana**: Kibana for viewing Elasticsearch logs
- **messagehub-prometheus**: Prometheus for OpenTelemetry
- **messagehub-grafana**: Grafana for visualizing data from OpenTelemetry


## Solution Structure

- **MessageHub.API**: The main web service providing an API for receiving messages and services for processing and sending messages.
- **MessageHub.Client**: A project for publishing a NuGet package that allows easy use of the MessageHub microservice in other services.
- **MessageHub.Core**: General and supporting functions.
- **MessageHub.Domain**: DTOs, entities, events, etc.
- **MessageHub.Infrastructure, MessageHub.Services, MessageHub.Shared**: Shared classes.
- **Providers Folder**: Contains individual providers for sending messages. Currently implemented: SendGrid, GoogleFirebase, EuroSMS, and for testing purposes, WebHook.com, which is also configured at the moment.
