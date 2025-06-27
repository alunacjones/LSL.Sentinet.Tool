using LSL.VariableReplacer;

namespace LSL.Sentinet.Tool.Cli.Configuration;

public interface ICommandProcessorFactory
{
    Task<CommandProcessingDelegate> BuildProcessor(string filePath, IVariableReplacer variableReplacer);
}
