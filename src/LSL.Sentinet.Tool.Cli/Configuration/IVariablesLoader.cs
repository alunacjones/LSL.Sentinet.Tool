namespace LSL.Sentinet.Tool.Cli.Configuration;

public interface IVariablesLoader
{
    Task<IDictionary<string, object>> LoadAsync(string configurationFilePath);
}
