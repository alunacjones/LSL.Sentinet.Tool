namespace LSL.Sentinet.Tool.Cli.Configuration;

public interface ITextFileFetcherFactory
{
    ITextFileFetcher Build(string basePath);
}
