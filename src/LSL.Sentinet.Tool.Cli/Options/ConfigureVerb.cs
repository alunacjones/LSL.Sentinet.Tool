namespace LSL.Sentinet.Tool.Cli.Options;

[Verb("configure", HelpText = "Hallo!")]
public class ConfigureVerb : BaseOptions
{
    [Option('f', "file", HelpText = "The configuration file to setup Sentinet with", Required = true)]
    public string Filename { get; set; } = default!;

    [Option('v', "variables", HelpText = "A list of variables to use", Required = false, Separator = ';')]
    public IEnumerable<string> Variables { get; set; } = Array.Empty<string>();
}
