using System;
using System.Threading.Tasks;

namespace Acme.Greeter.Services;

public class GreetingService
{
    private readonly QueueService _queueService;

    public GreetingService(QueueService queueService)
    {
        _queueService = queueService;
    }

    public async Task<string> Greet(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Argument must have a valid value", nameof(name));
        }

        var greeting = $"Hello, {name}";

        await _queueService.Send(name);

        return greeting;
    }
}