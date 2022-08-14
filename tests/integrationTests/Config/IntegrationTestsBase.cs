using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
namespace MiniBlog.Tests.Config;

public class IntegrationTestsBase : IClassFixture<CustomWebAppFactory<Program>>
{
    public readonly CustomWebAppFactory<Program> _factory;
    public readonly HttpClient _client;

    public IntegrationTestsBase(CustomWebAppFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions() { AllowAutoRedirect = false });

    }
    
}