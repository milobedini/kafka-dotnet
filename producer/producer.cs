using System;
using System.Threading;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


var config = new ProducerConfig
{
    BootstrapServers = "localhost:9092",
};

const string topic = "Users";

using var producer = new ProducerBuilder<Null, string>(config).Build();

try
{

    int objId = 0;
    while (objId <= 10)
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
        Thread.Sleep(3000);
        objId += 1;
    }

}
catch (ProduceException<Null, string> exc)
{
    Console.WriteLine(exc.Message);
    throw;
}

public record User(int UserId);