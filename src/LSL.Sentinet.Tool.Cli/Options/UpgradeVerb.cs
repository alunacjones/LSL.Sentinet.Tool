namespace LSL.Sentinet.Tool.Cli.Options;

[Verb("upgrade", HelpText = "Hallo!")]
public class UpgradeVerb : BaseOptions
{
    [Option('f', "file", HelpText = "Filename!", Required = true)]
    public string Filename { get; set; } = default!;
}
