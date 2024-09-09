using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsoleAppFactory;

public abstract class ConsolenApp()
{
    private const int ProgramExitError = -1;
    private const int ProgramExitSuccess = 0;
    private readonly IHost _app;
    private readonly ILogger<ConsolenApp> _logger;

    public ConsolenApp(IHost app) : this()
    {
        _app = app;
        _logger = _app.Services.GetService<ILogger<ConsolenApp>>();
    }

    public int Run()
    {
        try
        {
            _app.RunAsync();
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message, e);
            return ProgramExitError;
        }

        return ProgramExitSuccess;
    }
}