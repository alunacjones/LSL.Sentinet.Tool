using LSL.Evaluation.Jint;
using LSL.VariableReplacer;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace LSL.Sentinet.Tool.Cli.Configuration;

public class CommandProcessorFactory(
    IDeserializer deserializer,
    JintEvaluatorFactory jintEvaluatorFactory,
    ITextFileFetcherFactory textFileFetcherFactory) : ICommandProcessorFactory
{
    public async Task<CommandProcessingDelegate> BuildProcessor(string filePath)
    {
        using var commandReader = new StreamReader(filePath);

        var textFileFetcher = textFileFetcherFactory.Build(Path.GetDirectoryName(filePath)!);
        var commands = deserializer.Deserialize<CommandsContainer>(commandReader);
        var commandsCode = new List<string>();

        foreach (var commandFile in commands.CommandFiles)
        {
            commandsCode.Add(await textFileFetcher.FetchText(commandFile));
        }

        var evaluator = jintEvaluatorFactory.Build(c => commandsCode.ForEach(c.AddCode));

        return (command, value) => evaluator.Evaluate($"{command}({JsonConvert.SerializeObject(value)})").ToString();
    }
}
