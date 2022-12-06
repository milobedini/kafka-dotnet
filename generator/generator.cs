using System;
using System.Threading;
using Confluent.Kafka;
using Newtonsoft.Json;


const string topic = "incoming";

string kafkaBroker = Environment.GetEnvironmentVariable("KAFKA_BROKER");

var config = new ProducerConfig
{
    BootstrapServers = kafkaBroker,
    SecurityProtocol = SecurityProtocol.Plaintext,
};

using var producer = new ProducerBuilder<Null, string>(config).Build();

try
{
    Console.WriteLine($"Start generator {kafkaBroker}/{topic}");

    int objId = 0;
    while (objId < 10)
    {

        var response = await producer.ProduceAsync(topic,
        new Message<Null, string>
        {
            Value = JsonConvert.SerializeObject(
                new User(objId)
            )
        }
        );
        Console.WriteLine($"Sent {response.Value.Length} bytes to {topic} with the User ID of {objId}");
        Thread.Sleep(10000);
        objId += 1;
    }

}
catch (ProduceException<Null, string> exc)
{
    Console.WriteLine(exc.Message);
    throw;
}

public record User(int UserId);