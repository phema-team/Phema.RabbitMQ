# Phema.RabbitMQ

[![Build Status](https://cloud.drone.io/api/badges/phema-team/Phema.RabbitMQ/status.svg)](https://cloud.drone.io/phema-team/Phema.RabbitMQ)
[![Nuget](https://img.shields.io/nuget/v/Phema.RabbitMQ.svg)](https://www.nuget.org/packages/Phema.RabbitMQ)
[![Nuget](https://img.shields.io/nuget/dt/Phema.RabbitMQ.svg)](https://nuget.org/packages/Phema.RabbitMQ)

.NET Core strongly typed RabbitMQ integration library

## Concepts

- **Release immutability**
  - The topology for each release must be strictly defined and not changed during its existence
    - Until application is running you can't:
      - Cancel consumers
      - Remove queues and exchanges
      - Change bindings

- **Declarativeness and simplicity**
  - Intuitive RabbitMQ-close fluent interfaces
  - Generic type checks
  - Connection, Exchange, Queue, Consumer and Producer declaration classes

## Installation

```bash
  $> dotnet add package Phema.RabbitMQ
```

## Usage ([examples](https://github.com/phema-team/Phema.RabbitMQ/tree/master/examples))

```csharp
services.AddRabbitMQ(options =>
    options.UseConnectionFactory(factory => ...))
  .AddConnection(connection =>
  {
    var exchange = connection.AddDirectExchange("exchange")
      // .Internal()
      // .NoWait()
      // .Deleted()
      .AutoDelete()
      .Durable();

    var queue = connection.AddQueue<Payload>("queue")
      // .Exclusive()
      // .Deleted()
      // .NoWait()
      // .Lazy()
      // .MaxPriority(10)
      // .TimeToLive(10000)
      // .MaxMessageSize(1000)
      // .MaxMessageCount(1000)
      // .MessageTimeToLive(1000)
      // .RejectPublish()
      .AutoDelete()
      .Durable()
      .BoundTo(exchange);

    connection.AddConsumer(queue)
      // .Tagged("tag")
      // .Prefetch(1)
      // .Count(1)
      // .Exclusive()
      // .NoLocal()
      // .AutoAck()
      // .Requeue()
      // .Priority(2)
      .Count(2)
      .Requeue()
      .Subscribe(...);

    connection.AddProducer<Payload>(exchange)
      // .WaitForConfirms()
      // .Transactional()
      // .Mandatory()
      // .MessageTimeToLive(10000)
      .Persistent();
  });

// Get or inject
var producer = serviceProvider.GetRequiredService<IRabbitMQProducer>();

// Use
await producer.Publish(new Payload(), overrides => ...);
```

## Supported

- Durable, internal, dead letter, bound and alternate exchanges
- Lazy, durable and exclusive queues
- Default, confirm and transactional channel modes
- Persistent producers
- Consumers priority
- Queue and message time to live
- Max message count and size limitations
- Batch produce
- App id declaration
- Reject-publish when queue is full
- Deleted operations
- NoWait operations

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
- Add custom arguments with `Argument` extension
  
## Exchanges

- Declare durable exchange with `Durable` extension
- Declare exchange without waiting with `NoWait` extension
- Declare exchange as internal with `Internal` extension
- Delete exchange automatically with `AutoDelete` extension
- Explicitly delete exchange with `Deleted` extension
- Bind exchange to exchange with `BoundTo` extension
- Declare alternate exchange with `AlternateTo` extension
- Add custom arguments with `Argument` extension
- Declare exchange with `AddDirectExchange(...)`, `AddFanoutExchange(...)`, `AddHeadersExchange(...)`, `AddTopicExchange(...)` extensions

## Consumers

- Tag consumers using `Tagged` extension
- Limit prefetch count with `Prefetch` extension
- Scale consumers by using `Count` extension
- Declare exclusive consume with `Exclusive` extension
- Forbid to consume own producers with `NoLocal` extension
- When no need to ack explicitly use `AutoAck` extension
- Requeue messages on fail with `Requeue` extension
- Set consumer priority with `Priority` extension
- Add custom arguments with `Argument` extension
- All consumers start in `IHostedService`

## Producers

- Set routing key `RoutingKey` extension
- Set mandatory with `Mandatory` extension
- Set message priority with `Priority` extension
- Set message ttl with `MessageTimeToLive` extension
- Use channel transactional mode with `Transactional` extension
- Use channel confirm mode with `WaitForConfirms` extension
- Use message persistence with `Persistent` extension
- Configure headers with `Header` extension
- Configure properties with `Property` extension

## Limitations

- No dynamic topology declaration by design, but you can use `IRabbitMQConnectionProvider` for that
- No `correlation-id`'s
- No `message-id`'s
- No `BasicReturn` and other events for now

## Tips

- `RoutingKey` is `QueueName`/`ExchangeName` by default
- Do not use same connection for consumers and producers because of tcp backpressure
- Use `BatchProduce` instead of `Produce` with loop
