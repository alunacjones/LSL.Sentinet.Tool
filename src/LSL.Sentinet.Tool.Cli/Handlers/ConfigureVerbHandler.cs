using System.Text.Json;
using Microsoft.Extensions.Options;

namespace LSL.Sentinet.Tool.Cli.Handlers;

public class ConfigureVerbHandler : IAsyncHandler<ConfigureVerb>
{
    private readonly IConsole _console;
    private readonly ILogger<ConfigureVerbHandler> _logger;
    private readonly IOptionsMonitor<SentinetApiOptions> _options;

    public ConfigureVerbHandler(IConsole console, ILogger<ConfigureVerbHandler> logger, IOptionsMonitor<SentinetApiOptions> options)
    {
        _console = console;
        _logger = logger;
        _options = options;
    }

    public async Task<int> ExecuteAsync(ConfigureVerb options)
    {
        _console.WriteLine(JsonSerializer.Serialize(_options.CurrentValue));
        return 0;
    }
}