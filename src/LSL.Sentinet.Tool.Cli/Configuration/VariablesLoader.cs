using YamlDotNet.Serialization;

namespace LSL.Sentinet.Tool.Cli.Configuration;

public class VariablesLoader(IDeserializer deserializer) : IVariablesLoader
{
    public Task<IDictionary<string, object>> LoadAsync(string configurationFilePath)
    {
        using var reader = new StreamReader(configurationFilePath);

        return Task.FromResult<IDictionary<string, object>>(deserializer.Deserialize<VariablesContainer>(reader)
            .Variables
            .Select(v => new KeyValuePair<string, object>(v.Name, v.Value))
            .DistinctBy(v => v.Key)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
    }
}
