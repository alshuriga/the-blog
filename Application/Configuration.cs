
using Blog.Application.MappingProfiles;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Application;

public static class Configuration
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        services.AddMediatR(typeof(Configuration).Assembly);

        return services;
    }
}
