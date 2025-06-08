using System.Linq.Expressions;

namespace LSL.Sentinet.Tool.Cli.Options;

public abstract class BaseOptions : ICommandLineOptions
{
    [Option(HelpText = "The username to login with. If not provided then it falls back to the environment variable SENTINET_USERNAME")]
    public string Username { get; set; } = default!;

    [Option(HelpText = "The password to login with. If not provided then it falls back to the environment variable SENTINET_PASSWORD")]
    public string Password { get; set; } = default!;

    [Option(HelpText = "The base url of the Sentinet instance. If not provided then it falls back to the environment variable SENTINET_BASEURL")]
    public string BaseUrl { get; set; } = default!;

    public (string Username, string Password, string BaseUrl) GetLoginDetails() =>
        (
            GetValue(() => Username, "SENTINET_USERNAME"),
            GetValue(() => Password, "SENTINET_PASSWORD"),
            GetValue(() => BaseUrl, "SENTINET_BASEURL")
        );

    private static string GetValue(Expression<Func<string>> valueExpression, string environmentVariable)
    {
        var expression = (MemberExpression)valueExpression.Body;
        var name = expression.Member.Name;
        var value = valueExpression.Compile().Invoke();

        return value
            ?? Environment.GetEnvironmentVariable(environmentVariable)
            ?? throw new InvalidOperationException(
                $"Could not resolve the {name} from the provided options nor the environment variable {environmentVariable}"
            );
    }
}