using CommandLine;
using ConsoleAppFactory.CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsoleAppFactory;

public abstract class CreateConsoleApp
{
    private const string JsonFileEnding = ".json";
    private const string AppSettings = "appsettings";

    public IHost CreateHostBuilder<TService, TCommandLineOptions>(
        string[] args,
        TCommandLineOptions commandLineOptions,
        ParserSettings parserSettings)
        where TService : class, IHostedService
        where TCommandLineOptions : class
    {
        var appBuilder = Host.CreateApplicationBuilder(args);
        InitLogging(appBuilder);
        InitDependencyInjection(appBuilder, args, commandLineOptions, parserSettings);
        InitConfiguration(appBuilder, commandLineOptions);

        appBuilder.Services.AddHostedService<TService>();
        return appBuilder.Build();
    }

    private static void InitConfiguration<TCommandLineOptions>(
        HostApplicationBuilder appBuilder,
        TCommandLineOptions commandLineOptions)
        where TCommandLineOptions : class
    {
        appBuilder.Configuration.AddJsonFile($"{AppSettings}{JsonFileEnding}", true, true);
        appBuilder.Configuration.AddJsonFile(
            $"{AppSettings}.{Environment.GetEnvironmentVariable("ENVIRONMENT")}{JsonFileEnding}",
            true, true);
        appBuilder.Configuration.AddEnvironmentVariables();
        appBuilder.Configuration.Bind(commandLineOptions);
    }

    private void InitDependencyInjection<TCommandLineOptions>(
        HostApplicationBuilder appBuilder,
        string[] args,
        TCommandLineOptions commandLineOptions,
        ParserSettings parserSettings)
        where TCommandLineOptions : class
    {
        appBuilder.Services.AddCommandLineOptions(args, commandLineOptions, parserSettings);
        AddDependencyInjection(appBuilder);
    }

    protected abstract void AddDependencyInjection(HostApplicationBuilder appBuilder);

    private void InitLogging(HostApplicationBuilder appBuilder)
    {
        appBuilder.Logging.AddDebug();
        appBuilder.Logging.AddConsole();
    }
}