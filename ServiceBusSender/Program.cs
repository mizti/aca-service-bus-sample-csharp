using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using System.Text.Json;

string? connectionString = Environment.GetEnvironmentVariable("SERVICE_BUS_CONNECTION_STR");
string? queueName = Environment.GetEnvironmentVariable("SERVICE_BUS_QUEUE_NAME");

var data = new
{
    timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
    message = "hello, service bus!",
    key1 = "value1",
    key2 = "value2",
    key3 = "value3",
    nestedKey = new { nestedKey1 = "nestedValue1" },
    arrayKey = new[] { "arrayValue1", "arrayValue2" }
};

string jsonString = JsonSerializer.Serialize(data);

// since ServiceBusClient implements IAsyncDisposable we create it with "await using"
await using var client = new ServiceBusClient(connectionString);

// create the sender
ServiceBusSender sender = client.CreateSender(queueName);

// create a message that we can send. UTF-8 encoding is used when providing a string.
ServiceBusMessage message = new ServiceBusMessage(jsonString);

// send the message
await sender.SendMessageAsync(message);

Console.WriteLine("Send:\n");
Console.WriteLine(jsonString);
