using System;
using System.Threading;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

var config = new ConsumerConfig
{
    GroupId = "test-group",
    BootstrapServers = "localhost:9092",
    AutoOffsetReset = AutoOffsetReset.Earliest,
};

using var consumer = new ConsumerBuilder<Null, string>(config).Build();

string topic = "Users";

consumer.Subscribe(topic);

CancellationTokenSource token = new();

try
{

    while (true)
    {
        var response = consumer.Consume(token.Token);
        if (response.Message != null)
        {
            Console.WriteLine($"Read {response.Message.Value.Length} bytes from {topic}");
            var userObj = JsonConvert.DeserializeObject<User>
            (response.Message.Value);
            Console.WriteLine($"User ID: {userObj.UserId}");
        }
    }
}
catch (Exception)
{

    throw;
}

public record User(int UserId);