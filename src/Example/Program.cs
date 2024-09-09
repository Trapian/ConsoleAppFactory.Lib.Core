using CommandLine;
using Microsoft.Extensions.Hosting;

namespace ConsoleAppFactory.Example;

internal class Program
{
    private static void Main(string[] args)
    {
        var createTestApp = new CreateTestApp();
        var app = createTestApp.CreateHostBuilder<Service, Options>(args, new Options(), new ParserSettings());

        app.Run();
    }
}