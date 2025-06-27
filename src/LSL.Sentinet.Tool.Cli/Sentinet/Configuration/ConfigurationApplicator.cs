using LSL.Sentinet.Tool.Cli.Configuration;

namespace LSL.Sentinet.Tool.Cli.Sentinet.Configuration;

public class ConfigurationApplicator(ILogger<ConfigurationApplicator> logger) : IConfigurationApplicator
{
    public Task ConfigureSentinet(ConfigurationFile configuration)
    {
        logger.LogDebug("Started configuring Sentinet");

        logger.LogDebug("Finished configuring Sentinet");
        return Task.CompletedTask;
    }
}