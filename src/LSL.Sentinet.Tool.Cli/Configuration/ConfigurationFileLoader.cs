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
        var replacerWithPassedVariables = variableReplacer
            .CloneAndConfigure(c => c.AddPassedInVariables(variables));

        var commandProcessor = await commandProcessorFactory.BuildProcessor(filePath, replacerWithPassedVariables);
        var importedVariables = await variablesLoader.LoadAsync(filePath, replacerWithPassedVariables);

        var replacer = replacerWithPassedVariables.CloneAndConfigure(c => c
            .WithDefaultTransformer(commandProcessor: commandProcessor)
            .AddVariables(importedVariables)
            .ThrowIfVariableNotFound());

        using var reader = new StreamReader(filePath);

        return deserializer.DeserializeWithVariableReplacement<ConfigurationFile>(replacer, reader);
    }
}