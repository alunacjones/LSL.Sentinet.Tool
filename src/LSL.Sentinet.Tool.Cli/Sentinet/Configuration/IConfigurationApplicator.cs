using LSL.Sentinet.Tool.Cli.Configuration;

namespace LSL.Sentinet.Tool.Cli.Sentinet.Configuration;

public interface IConfigurationApplicator
{
    Task ConfigureSentinet(ConfigurationFile configuration);
}