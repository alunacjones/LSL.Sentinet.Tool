using LSL.Evaluation.Jint;
using LSL.VariableReplacer;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace LSL.Sentinet.Tool.Cli.Configuration;

public class CommandProcessorFactory(IDeserializer deserializer, JintEvaluatorFactory jintEvaluatorFactory) : ICommandProcessorFactory
{
    public async Task<CommandProcessingDelegate> BuildProcessor(string filePath)
    {
        using var commandReader = new StreamReader(filePath);

        var commands = deserializer.Deserialize<CommandsContainer>(commandReader);
        var basePath = Path.GetDirectoryName(filePath)!;

        var commandsCode = new List<string>();

        foreach (var command in commands.Commands)
        {
            commandsCode.Add(await File.ReadAllTextAsync(Path.Combine(basePath, command)));
        }

        var evaluator = jintEvaluatorFactory.Build(c => commandsCode.ForEach(c.AddCode));

        return (command, value) => evaluator.Evaluate($"{command}({JsonConvert.SerializeObject(value)})").ToString();
    }
}
