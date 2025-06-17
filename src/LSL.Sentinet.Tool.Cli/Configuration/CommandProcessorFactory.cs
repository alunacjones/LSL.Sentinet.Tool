using LSL.Evaluation.Jint;
using LSL.VariableReplacer;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace LSL.Sentinet.Tool.Cli.Configuration;

public class CommandProcessorFactory(
    IDeserializer deserializer,
    JintEvaluatorFactory jintEvaluatorFactory,
    HttpClient httpClient) : ICommandProcessorFactory
{
    public async Task<CommandProcessingDelegate> BuildProcessor(string filePath)
    {
        using var commandReader = new StreamReader(filePath);

        var commands = deserializer.Deserialize<CommandsContainer>(commandReader);
        var basePath = Path.GetDirectoryName(filePath)!;

        var commandsCode = new List<string>();

        foreach (var command in commands.Commands)
        {
            commandsCode.Add(await FetchFile(command, basePath));
        }

        var evaluator = jintEvaluatorFactory.Build(c => commandsCode.ForEach(c.AddCode));

        return (command, value) => evaluator.Evaluate($"{command}({JsonConvert.SerializeObject(value)})").ToString();
    }

    public async Task<string> FetchFile(string filePath, string baseFolder)
    {
        if (Uri.TryCreate(filePath, UriKind.Absolute, out var uri))
        {
            return await httpClient.GetStringAsync(uri.ToString());
        }

        return await File.ReadAllTextAsync(Path.Combine(baseFolder, filePath));
    }
}
