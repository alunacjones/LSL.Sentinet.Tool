using DotNetEnv;
using LSL.AbstractConsole.ServiceProvider;
using LSL.Evaluation.Jint;
using LSL.Sentinet.ApiClient.DependencyInjection;
using LSL.Sentinet.Tool.Cli.Configuration;
using LSL.VariableReplacer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YamlDotNet.Serialization;

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
        var builder = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(
                c => c.AddCommandLine(args, new Dictionary<string, string>
                    {
                        { "--username", "Sentinet:Username" },
                        { "--password", "Sentinet:Password" },
                        { "--baseurl", "Sentinet:BaseUrl" }
                    })
            );

        Env.TraversePath().Load();

        builder.ConfigureServices((hostContext, services) =>
        {
            var (isVerbose, filteredArguments) = ArgumentsPreprocessor.ProcessArguments(args);

            services
                .Configure<CommandLineOptions>(c => c.Arguments = filteredArguments)
                .AddScoped<ObfuscatingLogger>()
                .AddSingleton(_ => new VariableReplacerFactory()
                    .Build(c => c
                        .AddEnvironmentVariables(
                            e => e.WithEnvironmentVariableFilter(v => v.StartsWith("SENTINET_TOOL_")).WithPrefix(string.Empty)
                        ))
                )
                .AddHttpClient()
                .AddAutoFactory<ITextFileFetcherFactory>(c => c.AddConcreteType<ITextFileFetcher, TextFileFetcher>())
                .AddScoped(_ => new JintEvaluatorFactory())
                .AddScoped<IConfigurationFileLoader, ConfigurationFileLoader>()
                .AddScoped<ICommandProcessorFactory, CommandProcessorFactory>()
                .AddScoped<IVariablesLoader, VariablesLoader>()
                .AddScoped(_ => new DeserializerBuilder().IgnoreUnmatchedProperties().Build())
                .AddSentinetApiClient(
                    c => hostContext.Configuration.GetSection("Sentinet").Bind(c),
                        httpClientBuilderConfigurator: h => h.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
                        })
                        .RemoveAllLoggers()
                        .AddLogger<ObfuscatingLogger>()
                )
                .AddAbstractConsole()
                .AddCommandLineParser(typeof(Program).Assembly)
                .AddCliLogging(isVerbose);
        });

        return builder;
    }
}