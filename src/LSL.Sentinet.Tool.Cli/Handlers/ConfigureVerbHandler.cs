namespace LSL.Sentinet.Tool.Cli.Handlers;

public class ConfigureVerbHandler : IAsyncHandler<ConfigureVerb>
{
    private readonly IConsole _console;
    private readonly ILogger<ConfigureVerbHandler> _logger;

    public ConfigureVerbHandler(IConsole console, ILogger<ConfigureVerbHandler> logger)
    {
        _console = console;
        _logger = logger;
    }

    public async Task<int> ExecuteAsync(ConfigureVerb options)
    {
        var x = options.GetLoginDetails();
        _console.WriteLine($"{x}");
        return 0;
    }
}