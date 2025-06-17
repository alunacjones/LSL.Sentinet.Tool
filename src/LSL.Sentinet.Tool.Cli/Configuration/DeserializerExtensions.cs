using LSL.VariableReplacer;
using LSL.YamlDotNet.VariableReplacement;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace LSL.Sentinet.Tool.Cli.Configuration;

public static class DeserializerExtensions
{
    public static T DeserializeWithVariableReplacement<T>(this IDeserializer source, IVariableReplacer variableReplacer, TextReader textReader)
    {
        try
        {
            return source.Deserialize<T>(new VariableReplacerParser(variableReplacer.ReplaceVariables, new Y.Parser(textReader)));
        }
        catch (YamlException e) when (e.InnerException is not null)
        {
            throw e.InnerException;
        }
    }
}