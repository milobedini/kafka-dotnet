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

consumer.Subscribe("Users");

CancellationTokenSource token = new();

try
{

    while (true)
    {
        var response = consumer.Consume(token.Token);
        if (response.Message != null)
        {
            var userId = JsonConvert.DeserializeObject<User>
            (response.Message.Value);
            Console.WriteLine($"User ID: {userId}");
        }
    }
}
catch (Exception)
{

    throw;
}

public record User(int UserId);