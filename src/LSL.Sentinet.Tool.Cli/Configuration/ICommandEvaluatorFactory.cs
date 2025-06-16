using LSL.VariableReplacer;

namespace LSL.Sentinet.Tool.Cli.Configuration;

public interface ICommandEvaluatorFactory
{
    Task<CommandProcessingDelegate> BuildEvaluator(string filePath);
}
