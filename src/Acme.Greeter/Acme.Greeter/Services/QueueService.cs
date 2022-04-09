using System;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Options;

namespace Acme.Greeter.Services;

public class QueueService
{
    private readonly QueueSettings _settings;

    public QueueService(IOptions<QueueSettings> options)
    {
        _settings = options.Value;
    }

    public async Task Send(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Message cannot be empty");
        }

        var client = new QueueClient(_settings.ConnectionString, _settings.QueueName, new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });
        
        await client.CreateIfNotExistsAsync();
        await client.SendMessageAsync(message);
    }
}

public class QueueSettings
{
    public string ConnectionString { get; set; }
    public string QueueName { get; set; }
}