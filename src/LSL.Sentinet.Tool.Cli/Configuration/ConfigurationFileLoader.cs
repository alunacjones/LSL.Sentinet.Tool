using LSL.VariableReplacer;
using YamlDotNet.Serialization;

namespace LSL.Sentinet.Tool.Cli.Configuration;

public class ConfigurationFileLoader(
    IVariableReplacer variableReplacer,
    IVariablesLoader variablesLoader,
    IDeserializer deserializer,
    ICommandProcessorFactory commandProcessorFactory) : IConfigurationFileLoader
{
    /// <inheritdoc/>
    public async Task<ConfigurationFile> LoadAsync(string filePath, IEnumerable<string> variables)
    {
        var commandProcessor = await commandProcessorFactory.BuildProcessor(filePath);
        var importedVariables = await variablesLoader.LoadAsync(filePath);

        var replacer = variableReplacer.CloneAndConfigure(c => c
            .AddPassedInVariables(variables)
            .WithDefaultTransformer(commandProcessor: commandProcessor)
            .AddVariables(importedVariables)
            .ThrowIfVariableNotFound());

        using var reader = new StreamReader(filePath);

        return deserializer.DeserializeWithVariableReplacement<ConfigurationFile>(replacer, reader);
    }
}