using LSL.Sentinet.ApiClient.DependencyInjection;
using LSL.Sentinet.Tool.Cli.Configuration;
using LSL.Sentinet.Tool.Cli.Sentinet.Configuration;
using Microsoft.Extensions.Options;

namespace LSL.Sentinet.Tool.Cli.Handlers;

public class ConfigureVerbHandler(
    IConsole console,
    IOptions<SentinetApiOptions> sentinetOptions,
    IConfigurationFileLoader configurationFileLoader,
    IConfigurationApplicator configurationApplicator) : IAsyncHandler<ConfigureVerb>
{
    public async Task<int> ExecuteAsync(ConfigureVerb options)
    {
        var configFile = Path.GetFullPath(options.Filename);
        var startedAt = DateTime.Now;

        console.WriteLine($"Configuring Sentinet instance '{sentinetOptions.Value.BaseUrl}' from configuration file '{configFile}'");
        await configurationApplicator.ConfigureSentinet(await configurationFileLoader.LoadAsync(configFile, options.Variables));
        console.WriteLine($"Configuring Sentinet has completed successfully ({DateTime.Now.Subtract(startedAt).TotalSeconds}s)");

        return 0;
        // var folder = await foldersFacade.GetFolderAsync(Environment.GetEnvironmentVariable("SENTINET_TEST_PATH"));
        // var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
        // jsonOptions.Converters.Add(new JsonStringEnumConverter());
        // await File.WriteAllTextAsync("output.local.json", JsonSerializer.Serialize(folder, jsonOptions));
        // var svc = await foldersFacade.Client.GetServiceAsync(folder.SubTree.Services.OrderBy(s => s.Name.Length).First().Id);
        // var service = await foldersFacade.Client.GetServiceVersionAsync(svc.ServiceVersions.First().ServiceVersionId);
        // await File.WriteAllTextAsync("output2.local.json", JsonSerializer.Serialize(new { svc, service }, jsonOptions));

        // return 0;
    }
}