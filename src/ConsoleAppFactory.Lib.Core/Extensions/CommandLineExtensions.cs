using CommandLine;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleAppFactory.CommandLine;

public static class CommandLineExtensions
{
    public static IServiceCollection AddCommandLineOptions<TCommandLineOptions>(this IServiceCollection services,
        string[] args, TCommandLineOptions commandLineOptions, ParserSettings parserSettings)
        where TCommandLineOptions : class
    {
        return services
            .AddSingleton(commandLineOptions)
            .AddSingleton(new Arguments(args))
            .AddSingleton(parserSettings)
            .AddSingleton(typeof(IOptionsFactory<>),
                typeof(OptionsFactory<>));
    }
}