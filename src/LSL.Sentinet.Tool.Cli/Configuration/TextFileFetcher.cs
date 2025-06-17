
namespace LSL.Sentinet.Tool.Cli.Configuration;

public class TextFileFetcher(string basePath, HttpClient httpClient) : ITextFileFetcher
{
    public async Task<Stream> FetchStream(string path)
    {
        if (Uri.TryCreate(path, UriKind.Absolute, out var uri) && uri.Scheme is not "file")
        {
            var response = await httpClient.GetAsync(uri.ToString());
            return response.Content.ReadAsStream();
        }

        return File.OpenRead(Path.Combine(basePath, path));
    }
}