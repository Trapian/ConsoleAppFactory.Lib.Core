using Microsoft.Extensions.Hosting;

namespace ConsoleAppFactory.Example;

public class Service : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => Console.WriteLine("Hello World"));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => Console.WriteLine("Bye World"));
    }
}