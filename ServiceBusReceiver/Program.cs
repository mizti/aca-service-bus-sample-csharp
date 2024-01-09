using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

string? connectionString = Environment.GetEnvironmentVariable("SERVICE_BUS_CONNECTION_STR");
string? queueName = Environment.GetEnvironmentVariable("SERVICE_BUS_QUEUE_NAME");


// since ServiceBusClient implements IAsyncDisposable we create it with "await using"
await using var client = new ServiceBusClient(connectionString);

// create a receiver that we can use to receive the message
ServiceBusReceiver receiver = client.CreateReceiver(queueName);

// the received message is a different type as it contains some service set properties
ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();

// get the message body as a string
string body = receivedMessage.Body.ToString();
Console.WriteLine(body);

