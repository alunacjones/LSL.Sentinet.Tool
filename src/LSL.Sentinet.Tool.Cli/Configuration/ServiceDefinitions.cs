namespace LSL.Sentinet.Tool.Cli.Configuration;

public class ServiceDefinitions
{
    public IEnumerable<ServiceDefinition> Physical { get; set; } = [];
    public IEnumerable<ServiceDefinition> Virtual { get; set; } = [];
}
