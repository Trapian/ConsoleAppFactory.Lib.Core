using ConsoleAppFactory;
using Microsoft.Extensions.Hosting;

namespace ConsoleAppFactory.Example;

public class CreateTestApp : CreateConsoleApp
{
    protected override void AddDependencyInjection(HostApplicationBuilder appBuilder)
    {
    }
}