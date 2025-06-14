using Microsoft.Extensions.Http.Logging;

namespace LSL.Sentinet.Tool.Cli.Infrastructure;

internal class ObfuscatingLogger : IHttpClientLogger
{
    private readonly ILogger<ObfuscatingLogger> _logger;

    public ObfuscatingLogger(ILogger<ObfuscatingLogger> logger)
    {
        _logger = logger;
    }

    public void LogRequestFailed(object? context, HttpRequestMessage request, HttpResponseMessage? response, Exception exception, TimeSpan elapsed)
    {
        _logger.LogError(
            exception,
            "Request towards '{Request.Host}{Request.Path}' failed after {Response.ElapsedMilliseconds}ms",
            request.RequestUri?.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped),
            request.RequestUri!.PathAndQuery.RedactSensitiveInformation(),
            elapsed.TotalMilliseconds.ToString("F1"));
    }

    public object? LogRequestStart(HttpRequestMessage request)
    {
        _logger.LogInformation(
            "Sending '{Request.Method}' to '{Request.Host}{Request.Path}'",
            request.Method,
            request.RequestUri?.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped),
            request.RequestUri!.PathAndQuery.RedactSensitiveInformation());
        return null;
    }

    public void LogRequestStop(object? context, HttpRequestMessage request, HttpResponseMessage response, TimeSpan elapsed)
    {
        _logger.LogInformation(
            "Received '{Response.StatusCodeInt} {Response.StatusCodeString}' after {Response.ElapsedMilliseconds}ms",
            (int)response.StatusCode,
            response.StatusCode,
            elapsed.TotalMilliseconds.ToString("F1"));
    }
}
