namespace LSL.Sentinet.Tool.Cli.Options;

[Verb("configure", HelpText = "Hallo!")]
public class ConfigureVerb : BaseOptions
{
    [Option('f', "file", HelpText = "Filename!", Required = true)]
    public string Filename { get; set; } = default!;
}
