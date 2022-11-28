using System;
using System.Threading;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


var config = new ProducerConfig
{
    BootstrapServers = "localhost:9092",
};

string topic = "Users";

// Null as the key and string as the value
using var producer = new ProducerBuilder<Null, string>(config).Build();

// Just need to adapt consumer to act on a

try
{
    // string? firstName;
    // while ((firstName = Console.ReadLine()) != null)
    // Enter name into the terminal, which will be serialised and produce a new user object with the inputted first name.


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
        Console.WriteLine(response.Value);
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