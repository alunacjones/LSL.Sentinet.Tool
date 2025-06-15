namespace LSL.Sentinet.Tool.Cli.Configuration;

public interface IConfigurationFileLoader
{
    Task<ConfigurationFile> LoadAsync(string filePath, IEnumerable<string> variables);
}
