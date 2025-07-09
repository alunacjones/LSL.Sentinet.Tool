namespace LSL.Sentinet.Tool.Cli.Configuration;

public class ConfigurationFile
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Version { get; set; }
    public ServiceDefinitions Services { get; set; } = new();
}