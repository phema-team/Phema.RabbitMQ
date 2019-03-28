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
  - If no customers is needed, just do not add `Phema.RabbitMQ.Consumers` package
  - Each group has its own connection. Managing groups you manage connections

## Usage

```csharp
// Search for Phema.Serialization packages
services.AddPhemaJsonSerializer();

// Consumers
services.AddPhemaRabbitMQ("InstanceName", "amqp://connection.string")
  .AddQueueGroup(group =>
    group.AddQueue("QueueName")
      .Durable())
  .AddConsumerGroup(group =>
    group.AddConsumer<Payload, PayloadConsumer>("QueueName")
      .Count(2));

// Producers
services.AddPhemaRabbitMQ("InstanceName", factory => ...)
  .AddExchangeGroup(group =>
    group.AddDirectExchange("ExchangeName")
      .Durable())
  .AddProducerGroup(group =>
    group.AddProducer<Payload>("ExchangeName", "QueueName")
      .Persistent());
```

## Packages

- `Phema.RabbitMQ.Core` - Core factories, options and extensions
- `Phema.RabbitMQ.Exchanges` - Exchanges, `.AddExchangeGroup()` extension
- `Phema.RabbitMQ.Queues` - Queues, `.AddQueueGroup()` extension
- `Phema.RabbitMQ.Producers` - Producers, `.AddProducerGroup()` extension
- `Phema.RabbitMQ.Consumers` - Consumers, `.AddConsumerGroup()` extension
- `Phema.RabbitMQ` - Meta package

## Queues

- Declare durable queue with `Durable` extension
- Declare exclusive queue with `Exclusive` extension
- Declare queue without waiting with `NoWait` extension
- Bind exchange to exchange with `BoundTo` extension
- Declare lazy queue with `Lazy` extension
- Set queue max message count with `MaxMessageCount` extension
- Set queue max message size in bytes with `MaxMessageSize` extension
- Set dead letter exchange with `DeadLetterExchange` extension
- Set queue ttl with `WithTimeToLive` extension
- Set message ttl with `WithMessageTimeToLive` extension
- Set queue max priority with `WithMaxPriority` extension
- Explicitly delete queue with `Deleted` extension
- Delete queue automatically with `AutoDelete` extension
- Add custom arguments with `WithArgument` extension
  
## Exchanges

- Delete exchange automatically with `AutoDelete` extension
- Explicitly delete exchange with `Deleted` extension
- Bind exchange to exchange with `BoundTo` extension
- Add custom arguments with `WithArgument` extension
- Declare alternate exchange with `AlternateExchange` extension
- Declare durable exchange with `Durable` extension
- Declare exchange without waiting with `NoWait` extension
- Declare exchange as internal with `Internal` extension
- Declare exchange with `AddDirectExchange(...)`, `AddFanoutExchange(...)`, `AddHeadersExchange(...)`, `AddTopicExchange(...)` extensions

## Consumers

- Create `IRabbitMqConsumer<TPayload>`
- Tag consumers using `WithTag` extension
- Limit prefetch count with `PrefetchCount` extension
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

- Inject `IRabbitMQProdicer<TPayload>` and use `Produce` or `BatchProduce`
- Set routing key `RoutingKey` extension
- Set mandatory with `Mandatory` extension
- Set message priority with `Priority` extension
- Set message ttl with `MessageTimeToLive` extension
- Use channel transactional mode with `Transactional` extension
- Use channel confirm mode with `WaitForConfirms` extension
- Use message persistence with `Persistent` extension
- Configure headers with `WithHeader` extension
- Configure properties with `WithProperty` extension

## Limitations

- Uses only `Microsoft.Extensions.DepencencyInjection` package
- No dynamic topology declaration
  - No queues
  - No exchanges
  - No producers
  - No consumers
- No `.Redeclared()` and `.Purged()` because it breaks consistenty
  1. Deploy `first_node`
  2. Purge `queue`
  3. `first_node` starts produce messages
  4. Deploy `second_node`
  5. Purge `queue`
  6. No `first_node` messages survived
- There is a problem when one type of payload is used in different producers, so `IRabbitMQProducer<TPayload>` abstraction leak


## Supported

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

## Tips

- `RoutingKey` is `Queue.Name` by default
- Do not use same group(connection) for consumers and producers
- `IRabbitMQConnectionFactory` - singleton dependency in `IServiceProvider` per instance for custom connections
- If queue or exchange is default or exists, binding goes withot declaration
- Used `ISerializer` from `Phema.Serialization...` search for nuget package to add
