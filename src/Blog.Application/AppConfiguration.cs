
using Blog.Application.MappingProfiles;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Application;

public static class AppConfiguration
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        services.AddMediatR(typeof(AppConfiguration).Assembly);
        services.AddValidatorsFromAssemblyContaining(typeof(AppConfiguration));
        return services;
    }
}
