using CommandLine;

namespace ConsoleAppFactory.CommandLine;

internal class ParseException : Exception
{
    public ParseException(string? message, IEnumerable<Error> errors, Type type) : base(message)
    {
        Errors = errors;
        Type = type;
    }

    public IEnumerable<Error> Errors { get; }
    public Type Type { get; }
}