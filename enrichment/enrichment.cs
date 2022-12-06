using System;
using System.Threading;
using Confluent.Kafka;
using Newtonsoft.Json;

const string incomingTopic = "incoming";
const string outgoingTopic = "outgoing";

string kafkaBroker = Environment.GetEnvironmentVariable("KAFKA_BROKER");


var consumerConfig = new ConsumerConfig
{
    GroupId = "enrichment_service",
    BootstrapServers = kafkaBroker,
    AutoOffsetReset = AutoOffsetReset.Earliest,
    SecurityProtocol = SecurityProtocol.Plaintext,
    FetchWaitMaxMs = 5000,
};

var producerConfig = new ProducerConfig
{
    BootstrapServers = kafkaBroker,
    SecurityProtocol = SecurityProtocol.Plaintext,
};

using var consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();

using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();

consumer.Subscribe(incomingTopic);

CancellationTokenSource token = new();

try
{
    Console.WriteLine("Start enrichment");
    Console.WriteLine($"Incoming {kafkaBroker}/{incomingTopic}");
    Console.WriteLine($"Outgoing {kafkaBroker}/{outgoingTopic}");
    while (true)
    {
        // Consume the incoming message
        var consumeResponse = consumer.Consume(token.Token);
        if (consumeResponse.Message != null)
        {
            var userObj = JsonConvert.DeserializeObject<User>
            (consumeResponse.Message.Value);
            Console.WriteLine($"Read {consumeResponse.Message.Value.Length} bytes from {incomingTopic} with the User ID of {userObj.UserId}");

            // Enrich and produce the outgoing message
            var produceResponse = await producer.ProduceAsync(outgoingTopic,
            new Message<Null, string>
            {
                Value = JsonConvert.SerializeObject(
                    new User(userObj.UserId)
                )
            });
            Console.WriteLine($"Sent {produceResponse.Value.Length} bytes to {outgoingTopic} with the User ID of {userObj.UserId}");
        }
    }
}
catch (ProduceException<Null, string> exc)
{
    Console.WriteLine(exc.Message);
    throw;
}

public record User(int UserId, Null Key = null);