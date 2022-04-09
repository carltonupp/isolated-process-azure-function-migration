using System;
using Acme.Greeter.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Acme.Greeter.Startup))]

namespace Acme.Greeter;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddScoped<GreetingService>();
        builder.Services.AddScoped<QueueService>();

        builder.Services.Configure<QueueSettings>(options =>
        {
            var environmentVariables = Environment.GetEnvironmentVariables();
            var connectionString = environmentVariables["QueueConnectionString"]?.ToString();
            var queueName = environmentVariables["QueueName"]?.ToString();

            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(queueName))
            {
                throw new ApplicationException("Function cannot start without valid settings for queue storage.");
            }

            options.ConnectionString = connectionString;
            options.QueueName = queueName;
        });
    }
}