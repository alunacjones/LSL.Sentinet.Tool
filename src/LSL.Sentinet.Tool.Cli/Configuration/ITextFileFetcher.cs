namespace LSL.Sentinet.Tool.Cli.Configuration;

public interface ITextFileFetcher
{
    Task<Stream> FetchStream(string path);
}