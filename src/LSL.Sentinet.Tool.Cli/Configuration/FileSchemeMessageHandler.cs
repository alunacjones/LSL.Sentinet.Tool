

using System.Net;

namespace LSL.Sentinet.Tool.Cli.Configuration;

public class FileSchemeMessageHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri?.Scheme == "file")
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(await File.ReadAllTextAsync(request.RequestUri.LocalPath, cancellationToken))
            };
        }

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}