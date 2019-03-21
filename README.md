# Phema.RabbitMQ

```csharp
// Search for Phema.Serialization package
services.AddPhemaJsonSerializer();

// Consumers
services.AddPhemaRabbitMQ("InstanceName", "ConnectionString")
  .AddQueueGroup(group => group.AddQueue("QueueName"))
  .AddConsumerGroup(group => group.AddConsumer<Payload, PayloadConsumer>("QueueName"));

// Producers
services.AddPhemaRabbitMQ("InstanceName", factory => ...)
  .AddExchangeGroup(group => group.AddDirectExchange("ExchangeName"))
  .AddProducerGroup(group => group.AddProducer<Payload>("ExchangeName", "QueueName"));
```

- Packages
  - `Phema.RabbitMQ.Core` - Core builders and extensions
  - `Phema.RabbitMQ.Producers` - Producers and exchanges
  - `Phema.RabbitMQ.Consumers` - Consumers and queues
  - `Phema.RabbitMQ` - Both producers and consumers

- Queues
  - Declare durable queue with `Durable` extension
  - Declare exclusive queue with `Exclusive` extension
  - Declare queue without waiting with `NoWait` extension
  - Declare lazy queue with `Lazy` extension
  - Set queue max message count with `WithMaxMessageCount` extension
  - Set queue max message size in bytes with `WithMaxMessageSize` extension
  - Set dead letter exchange with `WithDeadLetterExchange` extension
  - Set dead letter routing key with `WithDeadLetterRoutingKey` extension
  - Set queue ttl with `WithTimeToLive` extension
  - Set message ttl with `WithMessageTimeToLive` extension
  - Set queue max priority with `WithMaxPriority` extension
  - Purge queue with `Purged` extension
  - Explicitly delete queue with `Deleted` extension
  - Delete queue automatically with `AutoDelete` extension
  - Add custom arguments with `WithArgument` extension
  
- Exchanges
  - Delete exchange automatically with `AutoDelete` extension
  - Explicitly delete exchange with `Deleted` extension
  - Bind exchange to exchange with `WithBoundExchange` extension
  - Add custom arguments with `WithArgument` extension
  - Declare alternate exchange with `WithAlternateExchange` extension
  - Declare durable exchange with `Durable` extension
  - Declare exchange without waiting with `NoWait` extension
  - Declare exchange as internal with `Internal` extension
  - Declare exchange with `AddDirectExchange(...)`, `AddFanoutExchange(...)`, `AddHeadersExchange(...)`, `AddTopicExchange(...)` extensions

- Consumers
  - Create `IRabbitMqConsumer<TPayload>`
  - Tag consumers using `WithTag` extension
  - Limit prefetch count with `WithPrefetchCount` extension
  - Scale consumers by using `WithCount` extension
  - Declare exclusive consume with `Exclusive` extension
  - Forbid to consume own producers with `NoLocal` extension
  - When no need to ack explicitly use `AutoAck` extension
  - Requeue messages on fail with `Requeue` extension
  - Set consumer priority with `WithPriority` extension
  - Add custom arguments with `WithArgument` extension
  - All consumers start in `IHostedService`
  - Use `IRabbitMQConsumerFactory` for custom message handling

- Producers
  - Inject `IRabbitMQProdicer<TPayload>` and use `Produce` or `BatchProduce`
  - Set routing key `WithRoutingKey` extension
  - Set mandatory with `Mandatory` extension
  - Set message priority with `WithPriority` extension
  - Set message ttl with `WithMessageTimeToLive` extension
  - Use channel transactional mode with `Transactional` extension
  - Use channel confirm mode with `WaitForConfirms` extension
  - Use message persistence with `Persistent` extension
  - Configure headers with `WithHeader` extension
  - Configure properties with `WithProperty` extension

- Supported
  - Consumers and producers priority
  - Queue and message time to live
  - Max message count and size limitations
  - Lazy, durable and exclusive queues
  - Batch produce
  - Durable, internal, dead letter, bound and alternate exchanges
  - Reject-publish when queue is full
  - Purge and delete declarative operations
  - Confirm and transactional channel modes
  - NoWait operations
  - Message persistency
  - Groups
    - Queue `QueueName` in `QueuesGroup` group will be `QueuesGroup.QueueName`)
    - Each group has own named connection

- Tips
  - `RoutingKey` is `Queue.Name` by default
  - Do not use same group(connection) for consumers and producers
  - `IRabbitMQConnectionFactory` - singleton dependency in `IServiceProvider` per instance for custom connections
  - If queue or exchange is default or exists, binding goes withot declaration
  - Used `ISerializer` from `Phema.Serialization...` search for nuget package to add
