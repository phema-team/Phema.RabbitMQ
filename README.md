# Phema.RabbitMQ



This is an attempt to create a simple way for safe and predictable application deploy with a versioned release-specific topology in a distributed systems

## Concepts

- **Release immutability**
  - The topology for each release must be strictly defined and not changed during its existence
    - There is no such thing as:
      - Canceling consumers
      - Removing queues and exchanges
      - Change bindings
- **Declarativeness and simplicity**
  - All parts are defined by describing their state
  - Intuitive, RabbitMQ-close fluent interfaces
  - Built-in modular serialization library, so working with objects, not bytes
- **Modularity and flexibility**
  - If no customers is needed, just do not add `Phema.RabbitMQ.Consumers` package, etc.
  - Each group has its own connection. Managing groups you manage connections

## Installation

```bash
  $> dotnet add package {{ PackageName }}
```

## Packages

- [![Nuget](https://img.shields.io/nuget/v/Phema.RabbitMQ.Core.svg)](https://www.nuget.org/packages/Phema.RabbitMQ.Core) `Phema.RabbitMQ.Core` - Core factories, options and extensions
- [![Nuget](https://img.shields.io/nuget/v/Phema.RabbitMQ.Exchanges.svg)](https://www.nuget.org/packages/Phema.RabbitMQ.Exchanges) `Phema.RabbitMQ.Exchanges` - Exchanges, `.AddExchangeGroup()` extension
- [![Nuget](https://img.shields.io/nuget/v/Phema.RabbitMQ.Queues.svg)](https://www.nuget.org/packages/Phema.RabbitMQ.Queues) `Phema.RabbitMQ.Queues` - Queues, `.AddQueueGroup()` extension
- [![Nuget](https://img.shields.io/nuget/v/Phema.RabbitMQ.Producers.svg)](https://www.nuget.org/packages/Phema.RabbitMQ.Producers) `Phema.RabbitMQ.Producers` - Producers, `.AddProducerGroup()` extension
- [![Nuget](https://img.shields.io/nuget/v/Phema.RabbitMQ.Consumers.svg)](https://www.nuget.org/packages/Phema.RabbitMQ.Consumers) `Phema.RabbitMQ.Consumers` - Consumers, `.AddConsumerGroup()` extension
- [![Nuget](https://img.shields.io/nuget/v/Phema.RabbitMQ.svg)](https://www.nuget.org/packages/Phema.RabbitMQ) `Phema.RabbitMQ` - Meta package

## Usage

```csharp
// Search for Phema.Serialization packages
services.AddNewtonsoftJsonSerializer();

// Consumers
services.AddRabbitMQ("InstanceName", "amqp://connection.string")
  .AddQueueGroup(group =>
    group.AddQueue("QueueName")
      .Durable())
  .AddConsumerGroup(group =>
    group.AddConsumer<Payload, PayloadConsumer>("QueueName")
      .Count(2));

// Producers
services.AddRabbitMQ("InstanceName", factory => ...)
  .AddExchangeGroup(group =>
    group.AddDirectExchange("ExchangeName")
      .Durable())
  .AddProducerGroup(group =>
    group.AddProducer<Payload>("ExchangeName", "QueueName")
      .Persistent());
```

## Supported

- Consumers and producers priority
- Queue and message time to live
- Max message count and size limitations
- Lazy, durable and exclusive queues
- Batch produce
- Declaring app id
- Durable, internal, dead letter, bound and alternate exchanges
- Reject-publish when queue is full
- Deleted declarative operation
- Default, confirm and transactional channel modes
- NoWait operations
- Message persistency
- Group-connections

## Queues

- Declare durable queue with `Durable` extension
- Declare exclusive queue with `Exclusive` extension
- Declare queue without waiting with `NoWait` extension
- Bind exchange to exchange with `BoundTo` extension
- Declare lazy queue with `Lazy` extension
- Set queue max message count with `MaxMessageCount` extension
- Set queue max message size in bytes with `MaxMessageSize` extension
- Set dead letter exchange with `DeadLetterTo` extension
- Set queue ttl with `TimeToLive` extension
- Set message ttl with `MessageTimeToLive` extension
- Set queue max priority with `MaxPriority` extension
- Explicitly delete queue with `Deleted` extension
- Delete queue automatically with `AutoDelete` extension
- Add custom arguments with `WithArgument` extension
  
## Exchanges

- Declare durable exchange with `Durable` extension
- Declare exchange without waiting with `NoWait` extension
- Declare exchange as internal with `Internal` extension
- Delete exchange automatically with `AutoDelete` extension
- Explicitly delete exchange with `Deleted` extension
- Bind exchange to exchange with `BoundTo` extension
- Declare alternate exchange with `AlternateTo` extension
- Add custom arguments with `WithArgument` extension
- Declare exchange with `AddDirectExchange(...)`, `AddFanoutExchange(...)`, `AddHeadersExchange(...)`, `AddTopicExchange(...)` extensions

## Consumers

- Declare scoped `IRabbitMqConsumer<TPayload>` with `Consume` method
- Tag consumers using `Tagged` extension
- Limit prefetch count with `Prefetch` extension
- Scale consumers by using `Count` extension
- Declare exclusive consume with `Exclusive` extension
- Forbid to consume own producers with `NoLocal` extension
- When no need to ack explicitly use `AutoAck` extension
- Requeue messages on fail with `Requeue` extension
- Set consumer priority with `Priority` extension
- Add custom arguments with `WithArgument` extension
- All consumers start in `IHostedService`
- Use `IRabbitMQConsumerFactory` for custom message handling

## Producers

- Inject scoped `IRabbitMQProdicer<TPayload>` and use `Produce` or `BatchProduce` methods
- Set routing key `RoutingKey` extension
- Set mandatory with `Mandatory` extension
- Set message priority with `Priority` extension
- Set message ttl with `MessageTimeToLive` extension
- Use channel transactional mode with `Transactional` extension
- Use channel confirm mode with `WaitForConfirms` extension
- Use message persistence with `Persistent` extension
- Configure headers with `WithHeader` extension
- Configure properties with `WithProperty` extension
- Use `IRabbitMQProducerFactory` for custom message producing

## Limitations

- Depends on `Microsoft.Extensions.DepencencyInjection` and `Phema.Serialization` package
- No dynamic topology declaration by design, but you can use `IRabbitMQConnectionFactory` for that ¯\_(ツ)_/¯
- No `.Redeclared()` and `.Purged()` because it breaks consistenty
  1. Deploy `first_node`
  2. Purge `queue`
  3. `first_node` starts produce messages
  4. Deploy `second_node`
  5. Purge `queue`
  6. No `first_node` messages survived
- There is a problem when one type of payload is used in different producers, so `IRabbitMQProducer<TPayload>` abstraction leak ;(
- No `correlation-id`'s
- No `message-id`'s

## Tips

- `RoutingKey` is `Queue.Name` by default
- Do not use same group(connection) for consumers and producers
- `IRabbitMQConnectionFactory` - singleton dependency in `IServiceProvider` per instance for custom connections
- If queue or exchange is default or exists, binding goes withot declaration
- Used `ISerializer` from `Phema.Serialization...` search for nuget package to add
