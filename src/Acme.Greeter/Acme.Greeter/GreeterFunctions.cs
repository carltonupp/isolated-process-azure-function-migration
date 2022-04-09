using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

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

    [Function("http-greeter")]
    public async Task<HttpResponseData> Greet(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Greet/{name?}")]
        HttpRequestData req, 
        string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            _logger.LogError("Path parameter 'name' is missing.");
            return req.CreateResponse(HttpStatusCode.BadRequest);
        }

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteStringAsync(await _greetingService.Greet(name));
        return response;
    }
    
    [Function("audit-greeting")]
    public void Audit(
        [QueueTrigger("audit-messages", Connection = "QueueConnectionString")] 
        string message)
    {
        _logger.LogTrace($"Greeting detected: {message}");
    }
}