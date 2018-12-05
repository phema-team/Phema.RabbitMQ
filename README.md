# Phema.Rabbit
AspNetCore Rabbit integration

```csharp

public class TestPayload
{
  public string Name { get; set; }
}

public class TestModelConsumer : RabbitConsumer<TestPayload>
{
  protected override string Tag => "TestModelConsumer";

  protected override async Task Consume(TestPayload payload)
  {
    await Console.Out.WriteLineAsync(payload.Name);
  }
}

public class TestPayloadProducer : RabbitProducer<TestPayload>
{
  public void Send(TestPayload testPayload)
  {
    Produce(testPayload);
  }
}

public class TestModelQueue : DurableRabbitQueue<TestPayload>
{
  protected override string Name => "TestModelQueue";
}

public class TestModelExchange : DirectRabbitExchange
{
  public override string Name => "TestModelExchange";
}

// Startup.cs
services.AddRabbit(options => { /*...*/ })
  .AddConsumers(consumers =>
    consumers.AddConsumer<TestPayload, TestModelConsumer, TestModelQueue>())
  .AddProducers(producers =>
    producers.AddProducer<TestPayload, TestPayloadProducer, TestModelQueue, TestModelExchange>());
```
