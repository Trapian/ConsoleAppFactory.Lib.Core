using CommandLine;

namespace ConsoleAppFactory.CommandLine;

public class OptionsFactory<TCommandLineOptions> : IOptionsFactory<TCommandLineOptions>
    where TCommandLineOptions : class, new()
{
    private readonly string[] _args;
    private readonly object _parserLock = new();
    private readonly ParserSettings _settings;
    private bool _hasParsed;

    public OptionsFactory(Arguments arguments, TCommandLineOptions commandLineOptions, ParserSettings settings)
    {
        _commandLineOptions = commandLineOptions;
        _settings = settings;
        _args = arguments.Args;
    }

    public static ParserSettings DefaultSettings => new()
    {
        AutoHelp = true,
        AutoVersion = false,
        CaseSensitive = false,
        PosixlyCorrect = true,
        CaseInsensitiveEnumValues = true,
        IgnoreUnknownArguments = true,
        HelpWriter = null
    };

    public TCommandLineOptions _commandLineOptions { get; }

    public TCommandLineOptions Value
    {
        get
        {
            lock (_parserLock)
            {
                if (!_hasParsed)
                {
                    ParseArguments();
                    _hasParsed = true;
                }

                return _commandLineOptions;
            }
        }
    }

    public void ParseArguments()
    {
        using var writer = new StringWriter();
        _settings.HelpWriter ??= writer;

        var parser = CreateParser();

        parser.ParseArguments<TCommandLineOptions>(_args)
            .WithNotParsed(errors => HandleErrors(errors, _settings.HelpWriter))
            .WithParsed(value => CopyValues(value, _commandLineOptions));
    }

    private void CopyValues(TCommandLineOptions source, TCommandLineOptions target)
    {
        var properties = typeof(TCommandLineOptions).GetProperties();
        foreach (var property in properties) property.SetValue(target, property.GetValue(source));
    }

    private Parser CreateParser()
    {
        var parser = new Parser(configuration =>
        {
            configuration.AutoVersion = _settings.AutoVersion;
            configuration.AutoHelp = _settings.AutoHelp;
            configuration.HelpWriter = _settings.HelpWriter;
            configuration.ParsingCulture = _settings.ParsingCulture;
            configuration.IgnoreUnknownArguments = _settings.IgnoreUnknownArguments;
            configuration.CaseSensitive = _settings.CaseSensitive;
            configuration.GetoptMode = _settings.GetoptMode;
            configuration.PosixlyCorrect = _settings.PosixlyCorrect;
            configuration.AllowMultiInstance = _settings.AllowMultiInstance;
            configuration.EnableDashDash = _settings.EnableDashDash;
            configuration.IgnoreUnknownArguments = _settings.IgnoreUnknownArguments;
            configuration.MaximumDisplayWidth = _settings.MaximumDisplayWidth;
            configuration.CaseInsensitiveEnumValues = _settings.CaseInsensitiveEnumValues;
        });

        return parser;
    }

    private static void HandleErrors(IEnumerable<Error> errors, TextWriter writer)
    {
        if (errors.Any(e => e.Tag != ErrorType.HelpRequestedError && e.Tag != ErrorType.VersionRequestedError))
        {
            var message = writer.ToString();
            throw new ParseException(message, errors, typeof(TCommandLineOptions));
        }
    }
}