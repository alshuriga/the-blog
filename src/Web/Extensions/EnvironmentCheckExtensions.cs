namespace Microsoft.Extensions.Hosting;

public static class EnvironmentCheckExtensions
{
    public static bool IsTesting(this IWebHostEnvironment env)
    {
        return env.EnvironmentName == "Testing";
    }
}