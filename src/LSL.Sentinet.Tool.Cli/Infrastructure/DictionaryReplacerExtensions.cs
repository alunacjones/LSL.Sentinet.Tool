using LSL.VariableReplacer;

namespace LSL.Sentinet.Tool.Cli.Infrastructure;

public static class DictionaryReplacerExtensions
{
    public static VariableReplacerConfiguration AddPassedInVariables(this VariableReplacerConfiguration configuration, IEnumerable<string> variables) => configuration.AddVariables(variables
        .Select(v =>
        {
            var split = v.Split('=');
            return new KeyValuePair<string, object>(split[0], split[1]);
        })
        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
}