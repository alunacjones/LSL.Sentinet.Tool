namespace LSL.Sentinet.Tool.Cli.Configuration;

public static class TextFileFetcherExtensions
{
    public static async Task<string> FetchText(this ITextFileFetcher textFileFetcher, string path)
    {
        using var stream = await textFileFetcher.FetchStream(path);
        using var reader = new StreamReader(stream);

        return await reader.ReadToEndAsync();
    }

    public static async Task<StreamReader> FetchStreamReader(this ITextFileFetcher textFileFetcher, string path) =>
        new StreamReader(await textFileFetcher.FetchStream(path));
}
