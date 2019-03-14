# Phema.RabbitMQ

```csharp
services.AddPhema...Serializer();

services.AddPhemaRabbitMQ("instance_name", factory => ...)
  .AddQueues(options =>
    options.AddQueue("queue_name")
      .Durable()
      .Exclusive()
      .AutoDelete()
      .WithMaxPriority(10)
      .WithTimeToLive(10_000)
      .WithMaxMessageCount(100)
      .WithMaxMessageSize(2048)
      .WithMessageTimeToLive(1000)
      .WithArgument("x-custom-argument", "some-value")
      .WithDeadLetterExchange("dead_letter_exchange")
      .WithDeadLetterRoutingKey("dead_letter_routing_key")
      .WithRejectPublishOnOverflow())
  .AddExchanges(options =>
    options.AddDirectExchange("exchange_name")
      .Durable()
      .AutoDelete()
      .WithAlternateExchange("alternate_exchange_name")
      .WithBoundExchange("bound_exchange", builder =>
        builder.WithRoutingKey("bound_exchange_routing_key")
          .WithArgument("x-custom-argument", "some-value")))
  .AddProducers(options =>
    options.AddProducer<Payload>("exchange_name", "queue_name")
      .Mandatory()
      .Persistent()
      .WithPriority(10)
      .WithTimeToLive(10_000)
      .WithHeader("header", "value")
      .WithRoutingKey("routing_key")
      .WithArgument("x-custom-argument", "some-value"))
  .AddConsumers(options =>
    options.AddConsumer<Payload, PayloadConsumer>("queue_name")
      .Exclusive()
      .Requeue()
      .AutoAck()
      .NoLocal()
      .WithCount(2)
      .WithPriority(5)
      .WithPrefetch(10)
      .WithTag("consumer_tag"));
```

- Packages
  - `Phema.RabbitMQ.Core` - Core builders and extensions
  - `Phema.RabbitMQ.Producers` - Producers and exchanges
  - `Phema.RabbitMQ.Consumers` - Consumers and queues
  - `Phema.RabbitMQ` - Both producers and consumers

- Exchanges
  - `AddDirectExchange(...)`  
  - `AddFanoutExchange(...)`
  - `AddHeadersExchange(...)`
  - `AddTopicExchange(...)`

- Consumers
  - Use `IRabbitMqConsumer<TPayload>`
  - All consumers start in `IHostedService`

- Tips
  - `IConnection` - singleton dependency in `IServiceProvider` per instance
  - Used `ISerializer` from `Phema.Serialization...` search for nuget package to add
  - `RoutingKey` is `Queue.Name` by default
  - Do not remove `DispatchConsumersAsync`