using LSL.VariableReplacer;
using YamlDotNet.Serialization;

namespace LSL.Sentinet.Tool.Cli.Configuration;

public class VariablesLoader(IDeserializer deserializer, ITextFileFetcherFactory textFileFetcherFactory) : IVariablesLoader
{
    public async Task<IDictionary<string, object>> LoadAsync(string configurationFilePath, IVariableReplacer variableReplacer)
    {
        var replacer = variableReplacer.CloneAndConfigure(c => c
            .WhenVariableNotFound(variableName => $"$({variableName})")
        );

        var textFileFetcher = textFileFetcherFactory.Build(Path.GetDirectoryName(configurationFilePath)!);

        return await Load(textFileFetcher, configurationFilePath);

        async Task<IDictionary<string, object>> Load(ITextFileFetcher textFileFetcher, string path)
        {
            using var reader = await textFileFetcher.FetchStreamReader(path);

            var variableDefinitions = deserializer.DeserializeWithVariableReplacement<VariablesContainer>(replacer, reader);
            var importedVariables = new Dictionary<string, object>();

            foreach (var import in variableDefinitions.Variables.Where(v => v.Import is not null))
            {
                importedVariables.Merge(await Load(textFileFetcher, import.Import));
            }

            return importedVariables.Merge(variableDefinitions
                .Variables
                .Where(v => v.Name is not null)
                .Select(v => new KeyValuePair<string, object>(v.Name, v.Value))
                .DistinctBy(v => v.Key)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }
    }
}

public static class DictionaryExtensions
{
    public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TValue> dictionaryToMergeFrom)
    {
        foreach (var item in dictionaryToMergeFrom)
        {
            dictionary[item.Key] = item.Value;
        }

        return dictionary;
    }
}