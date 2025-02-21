using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using StudentPortal.EventBus;
using RabbitMQ.Client;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Events;
using System.Text;

namespace StudentPortal.EventBusRabbitMQ;

public class EventBusOptions
{
    public string SubscriptionClientName { get; set; }
}

public class RabbitMQEventBus(
    ILogger<RabbitMQEventBus> logger,
    IServiceProvider serviceProvider,
    IOptions<EventBusOptions> options,
    IOptions<EventBusSubscriptionInfo> subscriptionOptions
    ) : IEventBus, IDisposable, IHostedService
{
    public const string EXCHANGE_NAME = "student_portal_event_bus";
    private IConnection _connection;
    private IChannel _channel;

    private readonly string _queueName = options.Value.SubscriptionClientName;

    public async Task PublishAsync(IntegrationEvent @event)
    {
        var routingKey = @event.GetType().Name;

        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace("Creating RabbitMQ channel to publish event: {EventId} ({EventType})", @event.Id, routingKey);
        }

        if (_connection is null)
        {
            throw new InvalidOperationException("Rabbit MQ connection is not opened");
        }

        using var channel = await _connection.CreateChannelAsync();

        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace("Declaring RabbitMQ exchange to publish event: {EventId}", @event.Id);
        }

        await channel.ExchangeDeclareAsync(exchange: EXCHANGE_NAME, type: "direct");

        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace("Publishing event to Rabbit MQ: {EventId}", @event.Id);
        }

        await channel.BasicPublishAsync(
            exchange: EXCHANGE_NAME,
            routingKey: routingKey,
            mandatory: true,
            body: SerializeMessage(@event)
        );
    }

    private byte[] SerializeMessage(IntegrationEvent @event)
    {
        return JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), subscriptionOptions.Value.JsonSerializerOptions);
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = Task.Factory.StartNew(async () =>
        {
            try
            {
                ArgumentNullException.ThrowIfNull(_queueName);
                logger.LogInformation("Starting Rabbit MQ connection");

                var factory = serviceProvider.GetRequiredService<ConnectionFactory>();
                _connection = await factory.CreateConnectionAsync();

                if (!_connection.IsOpen)
                {
                    return;
                }

                if (logger.IsEnabled(LogLevel.Trace))
                {
                    logger.LogTrace("Creating Rabbit MQ consumer channel");
                }

                _channel = await _connection.CreateChannelAsync();

                _channel.CallbackExceptionAsync += (sender, ea) =>
                {
                    logger.LogWarning(ea.Exception, "Error with Rabbit MQ consumer channel");
                    return Task.CompletedTask;
                };

                await _channel.ExchangeDeclareAsync(exchange: EXCHANGE_NAME, type: "direct");

                await _channel.QueueDeclareAsync(queue: _queueName, durable: true, exclusive: true, autoDelete: false, arguments: null);

                if (logger.IsEnabled(LogLevel.Trace))
                {
                    logger.LogTrace("Starting Rabbit MQ basic consume");
                }

                var consumer = new AsyncEventingBasicConsumer(_channel);

                consumer.ReceivedAsync += OnMessageReceived;

                await _channel.BasicConsumeAsync(
                    queue: _queueName,
                    autoAck: true,
                    consumer: consumer
                );

                foreach (var (eventName, _) in subscriptionOptions.Value.EventTypes)
                {
                    if (logger.IsEnabled(LogLevel.Trace))
                    {
                        logger.LogTrace("Registering routing key for event bus: {RoutingKey}", eventName);
                    }

                    await _channel.QueueBindAsync(
                        queue: _queueName,
                        exchange: EXCHANGE_NAME,
                        routingKey: eventName);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error starting RabbitMQ connection");
            }

        });

        return Task.CompletedTask;
    }

    private async Task ProcessEvent(string eventName, string message)
    {
        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);
        }

        await using var scope = serviceProvider.CreateAsyncScope();

        if (!subscriptionOptions.Value.EventTypes.TryGetValue(eventName, out var eventType))
        {
            logger.LogWarning("Unable to resolve event type for event name {EventName}", eventName);
            return;
        }

        var integrationEvent = DeserializeMessage(message, eventType);

        foreach (var handler in scope.ServiceProvider.GetKeyedServices<IIntegrationEventHandler>(eventType))
        {
            await handler.Handle(integrationEvent);
        }
    }

    private IntegrationEvent DeserializeMessage(string message, Type eventType)
    {
        return JsonSerializer.Deserialize(message, eventType, subscriptionOptions.Value.JsonSerializerOptions) as IntegrationEvent;
    }

    private async Task OnMessageReceived(object sender, BasicDeliverEventArgs eventArgs)
    {
        var eventName = eventArgs.RoutingKey;
        var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

        try
        {
            await ProcessEvent(eventName, message);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error processing message {Message}", message);
        }

        await _channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}