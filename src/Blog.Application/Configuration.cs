
using Blog.Application.MappingProfiles;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Application;

public static class Configuration
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        services.AddMediatR(typeof(Configuration).Assembly);
        services.AddValidatorsFromAssemblyContaining(typeof(Configuration));
        return services;
    }
}
