namespace LSL.Sentinet.Tool.Cli.Infrastructure;

public class BaseConfiguration : Dictionary<string, string?>, IBaseConfiguration
{
    public void UpdateFromBaseOptions(BaseOptions baseOptions)
    {
        this["Sentinet:BaseUrl"] = baseOptions.BaseUrl;
        this["Sentinet:Username"] = baseOptions.Username;
        this["Sentinet:Password"] = baseOptions.Password;        
    }
}