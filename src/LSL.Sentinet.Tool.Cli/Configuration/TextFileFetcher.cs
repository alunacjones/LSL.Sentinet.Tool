namespace LSL.Sentinet.Tool.Cli.Configuration;

public class TextFileFetcher(string basePath, HttpClient httpClient) : ITextFileFetcher
{
    public async Task<Stream> FetchStream(string path)
    {
        if (Uri.TryCreate(path, UriKind.Absolute, out var uri))
        {
            var response = await httpClient.GetAsync(uri.ToString());
            return response.ReadAsStreamIfSuccessful(path);
        }

        if (Uri.TryCreate(Path.Combine(basePath, path), UriKind.Absolute, out var uri2))
        {
            var response = await httpClient.GetAsync(uri2.ToString());
            return response.ReadAsStreamIfSuccessful(path);
        }

        throw new ArgumentException($"Could not resolve path '{path}' (relative to '{basePath}')");
    }
}