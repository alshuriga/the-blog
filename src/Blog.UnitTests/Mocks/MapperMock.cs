﻿using AutoMapper;
using Blog.Application.MappingProfiles;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Tests.Helpers
{
    public static class MapperMock
    {
        public static IMapper GetMapperServiceMock(Action<IServiceCollection>? func = null)
        {
            IServiceCollection services = new ServiceCollection();
            if (func != null) func.Invoke(services);
            services.AddAutoMapper(typeof(MappingProfile));

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<IMapper>();
        }
    }
}
