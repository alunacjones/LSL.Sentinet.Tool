namespace LSL.Sentinet.Tool.Cli.Handlers;

public class UpgradeVerbHandler : IAsyncHandler<UpgradeVerb>
{
    private readonly IConsole _console;
    private readonly ILogger<UpgradeVerbHandler> _logger;

    public UpgradeVerbHandler(IConsole console, ILogger<UpgradeVerbHandler> logger)
    {
        _console = console;
        _logger = logger;
    }

    public async Task<int> ExecuteAsync(UpgradeVerb options)
    {
        var x = options.GetLoginDetails();
        _console.WriteLine($"{x}");
        return 0;
    }
}