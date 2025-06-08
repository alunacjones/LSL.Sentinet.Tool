using DotNetEnv;
using LSL.AbstractConsole.ServiceProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LSL.Sentinet.Tool.Cli.Infrastructure;

public static class HostBuilderFactory
{
    /// <summary>
    /// Creates a host builder for the CLI
    /// </summary>
    /// <param name="args">The arguments passed into the CLI</param>
    /// <returns>The default host builder for the CLI</returns>
    public static IHostBuilder Create(string[] args)
    {
        var inMemorySettings = new BaseConfiguration();

        var builder = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(
                c => c.AddInMemoryCollection(inMemorySettings)
            );

        Env.TraversePath().Load();

        builder.ConfigureServices(services =>
        {
            var (isVerbose, filteredArguments) = ArgumentsPreprocessor.ProcessArguments(args);

            services
                .Configure<CommandLineOptions>(c => c.Arguments = filteredArguments)
                .AddAbstractConsole()
                .AddCommandLineParser(typeof(Program).Assembly)
                .AddHandlerInterceptors()
                .AddSingleton<IBaseConfiguration>(_ => inMemorySettings)
                .AddCliLogging(isVerbose);
        });

        return builder;
    }
}