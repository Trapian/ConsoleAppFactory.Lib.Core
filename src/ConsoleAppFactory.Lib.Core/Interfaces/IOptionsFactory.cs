using Microsoft.Extensions.Options;

namespace ConsoleAppFactory.CommandLine;

public interface IOptionsFactory<TCommandLineOptions> : IOptions<TCommandLineOptions>
    where TCommandLineOptions : class, new()
{
}