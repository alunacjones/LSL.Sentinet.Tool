namespace LSL.Sentinet.Tool.Cli.Configuration;

public static class HttpResponseMessageExtensions
{
    public static Stream ReadAsStreamIfSuccessful(this HttpResponseMessage httpResponseMessage, string path) =>
        httpResponseMessage.IsSuccessStatusCode ? httpResponseMessage.Content.ReadAsStream() : throw new ArgumentException($"Failed to retrieve '{path}'");       
}