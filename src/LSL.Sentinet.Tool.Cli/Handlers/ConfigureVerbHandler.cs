using System.Text.Json;
using System.Text.Json.Serialization;
using LSL.Sentinet.ApiClient.Facades;
using LSL.Sentinet.ApiClient.DependencyInjection;
using Microsoft.Extensions.Options;
using LSL.Sentinet.Tool.Cli.Configuration;

namespace LSL.Sentinet.Tool.Cli.Handlers;

public class ConfigureVerbHandler(
    IConsole console,
    ILogger<ConfigureVerbHandler> logger,
    IOptionsMonitor<SentinetApiOptions> options,
    IFoldersFacade foldersFacade,
    IConfigurationFileLoader configurationFileLoader) : IAsyncHandler<ConfigureVerb>
{
    public async Task<int> ExecuteAsync(ConfigureVerb options)
    {
        var file = options.Filename;
        var configuration = await configurationFileLoader.LoadAsync(options.Filename, options.Variables);

        return 0;
        var folder = await foldersFacade.GetFolderAsync(Environment.GetEnvironmentVariable("SENTINET_TEST_PATH"));
        var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
        jsonOptions.Converters.Add(new JsonStringEnumConverter());
        await File.WriteAllTextAsync("output.local.json", JsonSerializer.Serialize(folder, jsonOptions));
        var svc = await foldersFacade.Client.GetServiceAsync(folder.SubTree.Services.OrderBy(s => s.Name.Length).First().Id);
        var service = await foldersFacade.Client.GetServiceVersionAsync(svc.ServiceVersions.First().ServiceVersionId);
        await File.WriteAllTextAsync("output2.local.json", JsonSerializer.Serialize(new { svc, service }, jsonOptions));

        return 0;
    }
}