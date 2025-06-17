
namespace LSL.Sentinet.Tool.Cli.Configuration;

public class TextFileFetcher(string basePath, HttpClient httpClient) : ITextFileFetcher
{
    public async Task<string> FetchFile(string path)
    {
        if (Uri.TryCreate(path, UriKind.Absolute, out var uri))
        {
            return await httpClient.GetStringAsync(uri.ToString());
        }

        return await File.ReadAllTextAsync(Path.Combine(basePath, path));
    }
}