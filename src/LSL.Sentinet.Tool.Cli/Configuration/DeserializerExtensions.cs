using LSL.VariableReplacer;
using LSL.YamlDotNet.VariableReplacement;
using YamlDotNet.Serialization;

namespace LSL.Sentinet.Tool.Cli.Configuration;

public static class DeserializerExtensions
{
    public static T DeserializeWithVariableReplacement<T>(this IDeserializer source, IVariableReplacer variableReplacer, TextReader textReader) =>
        source.Deserialize<T>(new VariableReplacerParser(variableReplacer.ReplaceVariables, new Y.Parser(textReader)));
}