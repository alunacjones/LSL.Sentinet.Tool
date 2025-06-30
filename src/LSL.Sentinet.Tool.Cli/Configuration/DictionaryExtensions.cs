namespace LSL.Sentinet.Tool.Cli.Configuration;

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