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

        await _queueService.Send($"Greeting requested for {name}");
        
        return $"Hello, {name}";
    }
}