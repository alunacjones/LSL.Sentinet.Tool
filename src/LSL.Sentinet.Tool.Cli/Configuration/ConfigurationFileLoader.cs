using LSL.Evaluation.Jint;
using LSL.VariableReplacer;
using LSL.YamlDotNet.VariableReplacement;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace LSL.Sentinet.Tool.Cli.Configuration;

public class ConfigurationFileLoader(
    IVariableReplacer variableReplacer,
    IVariablesLoader variablesLoader,
    IDeserializer deserializer,
    JintEvaluatorFactory jintEvaluatorFactory) : IConfigurationFileLoader
{
    /// <inheritdoc/>
    public async Task<ConfigurationFile> LoadAsync(string filePath, IEnumerable<string> variables)
    {
        using var commandReader = new StreamReader(filePath);

        var commands = deserializer.Deserialize<CommandsContainer>(commandReader);
        var basePath = Path.GetDirectoryName(filePath)!;

        var evaluator = jintEvaluatorFactory.Build(c =>
        {
            foreach (var command in commands.Commands)
            {
                c.AddCode(File.ReadAllText(Path.Combine(basePath, command)));
            }
        });

        var replacer = variableReplacer.CloneAndConfigure(c => c
            .AddPassedInVariables(variables)
            .WithDefaultTransformer(commandProcessor:
                (command, value) => evaluator.Evaluate($"{command}({JsonConvert.SerializeObject(value)})").ToString()
            ));

        var importedVariables = await variablesLoader.LoadAsync(filePath, replacer);

        replacer = replacer.CloneAndConfigure(c => c.AddVariables(importedVariables));

        using var reader = new StreamReader(filePath);

        return deserializer.Deserialize<ConfigurationFile>(new VariableReplacerParser(replacer.ReplaceVariables, new Y.Parser(reader)));
    }
}

public interface ICommandsLoader
{

}

public class CommandsContainer
{
    public IEnumerable<string> Commands { get; set; } = [];
}