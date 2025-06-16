using LSL.VariableReplacer;
using YamlDotNet.Serialization;

namespace LSL.Sentinet.Tool.Cli.Configuration;

public class ConfigurationFileLoader(
    IVariableReplacer variableReplacer,
    IVariablesLoader variablesLoader,
    IDeserializer deserializer,
    ICommandEvaluatorFactory commandEvaluatorFactory) : IConfigurationFileLoader
{
    /// <inheritdoc/>
    public async Task<ConfigurationFile> LoadAsync(string filePath, IEnumerable<string> variables)
    {
        var commandEvaluator = await commandEvaluatorFactory.BuildEvaluator(filePath);

        var replacer = variableReplacer.CloneAndConfigure(c => c
            .AddPassedInVariables(variables)
            .WithDefaultTransformer(commandProcessor: commandEvaluator)
        );

        var importedVariables = await variablesLoader.LoadAsync(filePath, replacer);

        replacer = replacer.CloneAndConfigure(c => c.AddVariables(importedVariables));

        using var reader = new StreamReader(filePath);

        return deserializer.DeserializeWithVariableReplacement<ConfigurationFile>(replacer, reader);
    }
}