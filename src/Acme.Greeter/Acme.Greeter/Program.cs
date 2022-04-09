using System;
using Acme.Greeter.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddScoped<GreetingService>();
        services.AddScoped<QueueService>();

        services.Configure<QueueSettings>(options =>
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
    })
    .Build();

await host.RunAsync();