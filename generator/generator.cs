using System;
using System.Threading;
using Confluent.Kafka;
using Newtonsoft.Json;


const string topic = "incoming";
const string kafka_broker = "localhost:9092";

var config = new ProducerConfig
{
    BootstrapServers = kafka_broker,
    SecurityProtocol = SecurityProtocol.Plaintext,
};

using var producer = new ProducerBuilder<Null, string>(config).Build();

try
{
    Console.WriteLine($"Start generator {kafka_broker}/{topic}");

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