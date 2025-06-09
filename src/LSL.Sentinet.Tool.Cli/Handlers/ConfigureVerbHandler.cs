using System.Text.Json;
using LSL.Sentinet.ApiClient.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LSL.Sentinet.Tool.Cli.Handlers;

public class ConfigureVerbHandler : IAsyncHandler<ConfigureVerb>
{
    private readonly IConsole _console;
    private readonly ILogger<ConfigureVerbHandler> _logger;
    private readonly IOptionsMonitor<SentinetApiOptions> _options;
    private readonly IFoldersFacade _foldersFacade;

    public ConfigureVerbHandler(
        IConsole console,
        ILogger<ConfigureVerbHandler> logger,
        IOptionsMonitor<SentinetApiOptions> options,
        IFoldersFacade foldersFacade)
    {
        _console = console;
        _logger = logger;
        _options = options;
        _foldersFacade = foldersFacade;
    }

    public async Task<int> ExecuteAsync(ConfigureVerb options)
    {
        var folder = await _foldersFacade.GetFolderAsync(Environment.GetEnvironmentVariable("SENTINET_TEST_PATH"));
        await File.WriteAllTextAsync("output.local.json", JsonSerializer.Serialize(folder, new JsonSerializerOptions { WriteIndented = true }));
        var svc = await _foldersFacade.Client.GetServiceAsync(folder.SubTree.Services.OrderBy(s => s.Name.Length).First().Id);
        //var service = await _foldersFacade.Client.GetServiceVersionAsync(svc.ServiceVersions.First().ServiceVersionId);
        await File.WriteAllTextAsync("output2.local.json", JsonSerializer.Serialize(svc, new JsonSerializerOptions { WriteIndented = true }));

        return 0;
    }
}