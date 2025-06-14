using System.Text.RegularExpressions;

namespace LSL.Sentinet.Tool.Cli.Infrastructure;

internal static class LogOnRedactingStringExtensions
{
    private static readonly Regex _redactingRegex = new(@"password=.*", RegexOptions.Compiled);

    public static string RedactSensitiveInformation(this string pathAndQuery) =>
        pathAndQuery.Contains("/LogOn")
            ? _redactingRegex.Replace(pathAndQuery, string.Empty)
            : pathAndQuery;
}