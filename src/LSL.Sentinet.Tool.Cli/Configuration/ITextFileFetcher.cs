namespace LSL.Sentinet.Tool.Cli.Configuration;

public interface ITextFileFetcher
{
    Task<string> FetchFile(string path);
}
