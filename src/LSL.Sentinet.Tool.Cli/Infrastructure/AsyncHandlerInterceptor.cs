using Castle.DynamicProxy;
using Microsoft.Extensions.Configuration;

namespace LSL.Sentinet.Tool.Cli.Infrastructure;

public class AsyncHandlerInterceptor : AsyncInterceptorBase
{
    private readonly IConfiguration _configuration;
    private readonly IBaseConfiguration _baseConfiguration;

    public AsyncHandlerInterceptor(IConfiguration configuration, IBaseConfiguration baseConfiguration)
    {
        _configuration = configuration;
        _baseConfiguration = baseConfiguration;
    }

    protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
    {
        UpdateConfiguration(invocation);
        await proceed(invocation, proceedInfo);
    }

    protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
    {
        UpdateConfiguration(invocation);
        return await proceed(invocation, proceedInfo);
    }

    private void UpdateConfiguration(IInvocation invocation)
    {
        var baseOptions = invocation.Arguments.OfType<BaseOptions>().FirstOrDefault();

        if (baseOptions != null)
        {
            _baseConfiguration.UpdateFromBaseOptions(baseOptions);
            ((IConfigurationRoot)_configuration).Reload();
        }        
    }
}