namespace LSL.Sentinet.Tool.Cli.Configuration;

public class ServiceDefinition
{
    public string Name { get; set; } = default!;
    public IEnumerable<ServiceVersion> Versions { get; set; } = [];
}
