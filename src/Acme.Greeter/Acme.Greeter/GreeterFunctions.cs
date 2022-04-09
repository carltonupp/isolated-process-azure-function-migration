using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue.Protocol;

namespace Acme.Greeter;

using Services;

public class GreeterFunctions
{
    private readonly GreetingService _greetingService;
    private readonly ILogger<GreeterFunctions> _logger;

    public GreeterFunctions(GreetingService greetingService, 
        ILogger<GreeterFunctions> logger)
    {
        _greetingService = greetingService;
        _logger = logger;
    }

    [FunctionName("http-greeter")]
    public async Task<IActionResult> Greet(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Greet/{name?}")] 
        HttpRequest req, 
        string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            _logger.LogError("Path parameter 'name' is missing.");
            return new BadRequestResult();
        }
        
        return new OkObjectResult(await _greetingService.Greet(name));
    }
    
    [FunctionName("audit-greeting")]
    public void Audit(
        [QueueTrigger("audit-messages", Connection = "QueueConnectionString")] 
        string message)
    {
        _logger.LogTrace($"Greeting detected: {message}");
    }
}